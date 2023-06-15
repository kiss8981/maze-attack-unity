using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField, Header("미로 크기")]
    public Vector2Int mazeSize = new Vector2Int(25, 25);

    private Vector2Int BlockSize => mazeSize / 2;

    private Block[,] blocks;

    [HideInInspector]
    public bool[,] existWalls;
    private DisjointSet disjointSet;
    private readonly Dictionary<int, List<int>> lastRowBlocks = new Dictionary<int, List<int>>();

    [SerializeField, Header("생성할 벽 오브젝트")]
    private GameObject wallPrefab;

    public void MazeGenerate(int width, int height)
    {
        mazeSize = new Vector2Int(width, height);
        InitMazeSetting();
        GenerateMaze();
    }

    private void InitMazeSetting()
    {
        Block.ResetBlockNumber();

        var size = BlockSize;
        var disjointSetSize = BlockSize.x * BlockSize.y;

        blocks = new Block[size.x, size.y];
        existWalls = new bool[mazeSize.x, mazeSize.y];
        disjointSet = new DisjointSet(disjointSetSize);
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
        BuildWalls();
    }

    private void InitBlocks()
    {
        for (int x = 0; x < BlockSize.x; x++)
        {
            for (int y = 0; y < BlockSize.y; y++)
            {
                blocks[x, y] = new Block();
            }
        }
    }

    private void RandomMergeRowBlocks(int row)
    {
        for (int x = 0; x < BlockSize.x - 1; x++)
        {
            var canMerge = Random.Range(0, 2) == 1;
            var currentBlockNumber = blocks[x, row].BlockNumber;
            var nextBlockNumber = blocks[x + 1, row].BlockNumber;

            if (canMerge && !disjointSet.IsUnion(currentBlockNumber, nextBlockNumber))
            {
                disjointSet.Merge(currentBlockNumber, nextBlockNumber);
                blocks[x, row].OpenWay[(int)Direction.Right] = true;
            }
        }
    }

    private void DropDownGroups(int row)
    {
        lastRowBlocks.Clear();

        for (int x = 0; x < BlockSize.x; x++)
        {
            int blockNumber = blocks[x, row].BlockNumber;
            int parentNumber = disjointSet.Find(blockNumber);

            if (!lastRowBlocks.ContainsKey(parentNumber))
            {
                lastRowBlocks.Add(parentNumber, new List<int>());
            }

            lastRowBlocks[parentNumber].Add(blockNumber);
        }

        foreach (var lastRowBlock in lastRowBlocks)
        {
            if (lastRowBlock.Value.Count == 0)
                continue;

            var randomDownCount = Random.Range(1, lastRowBlock.Value.Count);

            for (int i = 0; i < randomDownCount; i++)
            {
                var randomBlockIndex = Random.Range(0, lastRowBlock.Value.Count);

                var currentBlockNumber = lastRowBlock.Value[randomBlockIndex];
                var currentBlockPosition = Block.GetPosition(currentBlockNumber, BlockSize);

                var currentBlock = blocks[currentBlockPosition.x, currentBlockPosition.y];
                var underBlock = blocks[currentBlockPosition.x, currentBlockPosition.y + 1];

                disjointSet.Merge(currentBlock.BlockNumber, underBlock.BlockNumber);
                currentBlock.OpenWay[(int)Direction.Down] = true;

                lastRowBlock.Value.RemoveAt(randomBlockIndex);
            }
        }
    }

    private void OrganizeLastLine()
    {
        var lastRow = BlockSize.y - 1;

        for (int x = 0; x < BlockSize.x - 1; x++)
        {
            var currentBlock = blocks[x, lastRow];
            var nextBlock = blocks[x + 1, lastRow];

            if (!disjointSet.IsUnion(currentBlock.BlockNumber, nextBlock.BlockNumber))
            {
                currentBlock.OpenWay[(int)Direction.Right] = true;
            }
        }
    }

    private void MakeHoleInPath()
    {
        for (int x = 0; x < BlockSize.x; x++)
        {
            for (int y = 0; y < BlockSize.y; y++)
            {
                var adjustPosition = new Vector2Int(x * 2 + 1, y * 2 + 1);
                existWalls[adjustPosition.x, adjustPosition.y] = true;

                if (blocks[x, y].OpenWay[(int)Direction.Down])
                    existWalls[adjustPosition.x, adjustPosition.y + 1] = true;
                if (blocks[x, y].OpenWay[(int)Direction.Right])
                    existWalls[adjustPosition.x + 1, adjustPosition.y] = true;
            }
        }
    }

    private void BuildWalls()
    {
        for (int x = 0; x < mazeSize.x; x++)
        {
            for (int y = 0; y < mazeSize.y; y++)
            {
                if (existWalls[x, y])
                    continue;

                var mazeHalfSize = new Vector3(mazeSize.x, mazeSize.y, 0) / 2;
                var wallPosition = new Vector3(x, y, 0) - mazeHalfSize + transform.position;

                Instantiate(wallPrefab, wallPosition, Quaternion.identity, transform);
            }
        }
    }
}
