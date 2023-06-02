using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public Vector2Int mazeSize = new Vector2Int(25, 25);

    private Vector2Int BlockSize => mazeSize / 2;

    private Block[,] _blocks;
    private bool[,] _existWalls;
    private DisjointSet _disjointSet;
    private readonly Dictionary<int, List<int>> _lastRowBlocks = new Dictionary<int, List<int>>();

    [SerializeField]
    private float delayCreateTime = 0.25f;

    [SerializeField]
    private bool isDelayCreate;

    [SerializeField]
    private bool isDrawGizmo;

    [SerializeField]
    private GameObject wallPrefab;

    public void MazeGenerate(int width, int height)
    {
        mazeSize = new Vector2Int(width, height);
        InitMazeSetting();
        GenerateMaze();
    }

    private void InitMazeSetting()
    {
        var size = BlockSize;
        var disjointSetSize = BlockSize.x * BlockSize.y;

        _blocks = new Block[size.x, size.y];
        _existWalls = new bool[mazeSize.x, mazeSize.y];
        _disjointSet = new DisjointSet(disjointSetSize);
    }

    public Vector2 getStartBlockNumber()
    {
        return new Vector2(0, 0);
    }

    public Vector2 getEndBlockNumber()
    {
        return new Vector2(BlockSize.x - 1, BlockSize.y - 1);
    }

    private void GenerateMaze()
    {
        InitBlocks();

        for (int y = 0; y < BlockSize.y - 1; y++)
        {
            RandomMergeRowBlocks(y);
            DropDownGroups(y);
        }

        OrganizeLastLine();
        MakeHoleInPath();

        if (!isDrawGizmo)
        {
            BuildWalls();
        }
    }

    private void InitBlocks()
    {
        for (int x = 0; x < BlockSize.x; x++)
        {
            for (int y = 0; y < BlockSize.y; y++)
            {
                _blocks[x, y] = new Block();
            }
        }
    }

    private void RandomMergeRowBlocks(int row)
    {
        for (int x = 0; x < BlockSize.x - 1; x++)
        {
            var canMerge = Random.Range(0, 2) == 1;
            var currentBlockNumber = _blocks[x, row].BlockNumber;
            var nextBlockNumber = _blocks[x + 1, row].BlockNumber;

            if (canMerge && !_disjointSet.IsUnion(currentBlockNumber, nextBlockNumber))
            {
                _disjointSet.Merge(currentBlockNumber, nextBlockNumber);
                _blocks[x, row].OpenWay[(int)Direction.Right] = true;
            }
        }
    }

    private void DropDownGroups(int row)
    {
        _lastRowBlocks.Clear();

        for (int x = 0; x < BlockSize.x; x++)
        {
            var blockNumber = _blocks[x, row].BlockNumber;
            var parentNumber = _disjointSet.Find(_blocks[x, row].BlockNumber);

            if (!_lastRowBlocks.ContainsKey(parentNumber))
            {
                _lastRowBlocks.Add(parentNumber, new List<int>());
            }

            _lastRowBlocks[parentNumber].Add(blockNumber);
        }

        foreach (var group in _lastRowBlocks)
        {
            if (group.Value.Count == 0)
                continue;

            var randomDownCount = Random.Range(1, group.Value.Count);

            for (int i = 0; i < randomDownCount; i++)
            {
                var randomBlockIndex = Random.Range(0, group.Value.Count);

                var currentBlockNumber = group.Value[randomBlockIndex];
                var currentBlockPosition = Block.GetPosition(currentBlockNumber, BlockSize);

                var currentBlock = _blocks[currentBlockPosition.x, currentBlockPosition.y];
                var underBlock = _blocks[currentBlockPosition.x, currentBlockPosition.y + 1];

                _disjointSet.Merge(currentBlock.BlockNumber, underBlock.BlockNumber);
                currentBlock.OpenWay[(int)Direction.Down] = true;

                group.Value.RemoveAt(randomBlockIndex);
            }
        }
    }

    private void OrganizeLastLine()
    {
        var lastRow = BlockSize.y - 1;

        for (int x = 0; x < BlockSize.x - 1; x++)
        {
            var currentBlock = _blocks[x, lastRow];
            var nextBlock = _blocks[x + 1, lastRow];

            if (!_disjointSet.IsUnion(currentBlock.BlockNumber, nextBlock.BlockNumber))
            {
                currentBlock.OpenWay[(int)Direction.Right] = true;
            }
        }
    }

    private IEnumerator DelayCreateMaze()
    {
        for (int y = 0; y < BlockSize.y - 1; y++)
        {
            yield return StartCoroutine(DelayRandomMergeBlocks(y));
            yield return StartCoroutine(DelayDropDownGroups(y));

            MakeHoleInPath();

            yield return new WaitForSeconds(delayCreateTime);
        }

        yield return new WaitForSeconds(delayCreateTime);

        yield return StartCoroutine(DelayCleanUpLastLine());
        MakeHoleInPath();
    }

    private IEnumerator DelayRandomMergeBlocks(int row)
    {
        for (int x = 0; x < BlockSize.x - 1; x++)
        {
            var canMerge = Random.Range(0, 2) == 1;
            var currentBlockNumber = _blocks[x, row].BlockNumber;
            var nextBlockNumber = _blocks[x + 1, row].BlockNumber;

            if (canMerge && !_disjointSet.IsUnion(currentBlockNumber, nextBlockNumber))
            {
                _disjointSet.Merge(currentBlockNumber, nextBlockNumber);
                _blocks[x, row].OpenWay[(int)Direction.Right] = true;
            }

            MakeHoleInPath();

            yield return new WaitForSeconds(delayCreateTime);
        }
    }

    private IEnumerator DelayDropDownGroups(int row)
    {
        _lastRowBlocks.Clear();

        for (int x = 0; x < BlockSize.x; x++)
        {
            var blockNumber = _blocks[x, row].BlockNumber;
            var parentNumber = _disjointSet.Find(_blocks[x, row].BlockNumber);

            if (!_lastRowBlocks.ContainsKey(parentNumber))
            {
                _lastRowBlocks.Add(parentNumber, new List<int>());
            }

            _lastRowBlocks[parentNumber].Add(blockNumber);
        }

        foreach (var group in _lastRowBlocks)
        {
            if (group.Value.Count == 0)
                continue;

            var randomDownCount = Random.Range(1, group.Value.Count);

            for (int i = 0; i < randomDownCount; i++)
            {
                var randomBlockIndex = Random.Range(0, group.Value.Count);

                var currentBlockNumber = group.Value[randomBlockIndex];
                var currentBlockPosition = Block.GetPosition(currentBlockNumber, BlockSize);

                var currentBlock = _blocks[currentBlockPosition.x, currentBlockPosition.y];
                var underBlock = _blocks[currentBlockPosition.x, currentBlockPosition.y + 1];

                _disjointSet.Merge(currentBlock.BlockNumber, underBlock.BlockNumber);
                currentBlock.OpenWay[(int)Direction.Down] = true;

                group.Value.RemoveAt(randomBlockIndex);

                MakeHoleInPath();

                yield return new WaitForSeconds(delayCreateTime);
            }
        }
    }

    private IEnumerator DelayCleanUpLastLine()
    {
        var lastRow = BlockSize.y - 1;

        for (int x = 0; x < BlockSize.x - 1; x++)
        {
            var currentBlock = _blocks[x, lastRow];
            var nextBlock = _blocks[x + 1, lastRow];

            if (!_disjointSet.IsUnion(currentBlock.BlockNumber, nextBlock.BlockNumber))
            {
                currentBlock.OpenWay[(int)Direction.Right] = true;
            }

            MakeHoleInPath();

            yield return new WaitForSeconds(delayCreateTime);
        }
    }

    private void MakeHoleInPath()
    {
        for (int x = 0; x < BlockSize.x; x++)
        {
            for (int y = 0; y < BlockSize.y; y++)
            {
                var adjustPosition = new Vector2Int(x * 2 + 1, y * 2 + 1);
                _existWalls[adjustPosition.x, adjustPosition.y] = true;

                if (_blocks[x, y].OpenWay[(int)Direction.Down])
                    _existWalls[adjustPosition.x, adjustPosition.y + 1] = true;
                if (_blocks[x, y].OpenWay[(int)Direction.Right])
                    _existWalls[adjustPosition.x + 1, adjustPosition.y] = true;
            }
        }
    }

    private void BuildWalls()
    {
        for (int x = 0; x < mazeSize.x; x++)
        {
            for (int y = 0; y < mazeSize.y; y++)
            {
                if (_existWalls[x, y])
                    continue;

                var myTransform = transform;
                var mazeHalfSize = new Vector3(mazeSize.x, mazeSize.y, 0) / 2;
                var wallPosition = new Vector3(x, y, 0) - mazeHalfSize + myTransform.position;

                Instantiate(wallPrefab, wallPosition, Quaternion.identity, myTransform);
            }
        }
    }

    public Transform GetMazeTransform()
    {
        return transform;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying && isDrawGizmo)
        {
            Gizmos.color = Color.red;

            for (int x = 0; x < mazeSize.x; x++)
            {
                for (int y = 0; y < mazeSize.y; y++)
                {
                    if (!_existWalls[x, y])
                    {
                        var mazeHalfSize = new Vector3(mazeSize.x, mazeSize.y, 0) / 2;
                        var wallPosition = new Vector3(x, y, 0) - mazeHalfSize + transform.position;
                        Gizmos.DrawCube(wallPosition, Vector3.one);
                    }
                }
            }
        }
    }
}
