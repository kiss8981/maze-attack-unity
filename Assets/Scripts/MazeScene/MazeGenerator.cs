using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeGenerator : MonoBehaviour
{
    public int mazeWidth;
    public int mazeHeight;
    public GameObject wallPrefab;
    public GameObject startPrefab;
    public GameObject endPrefab;
    public GameObject outerWallPrefab;
    public float wallSize = 1f;
    public float delay = 0.1f; // 각 스텝의 시간 지연

    private int[,] maze;
    private Vector2Int startCell;
    private Vector2Int endCell;

    void Start()
    {
        StartCoroutine(GenerateMazeCoroutine());
    }

    IEnumerator GenerateMazeCoroutine()
    {
        // 초기화
        maze = new int[mazeHeight, mazeWidth];

        // 외벽 생성
        for (int y = 0; y < mazeHeight; y++)
        {
            maze[y, 0] = -1;
            maze[y, mazeWidth - 1] = -1;
        }

        for (int x = 0; x < mazeWidth; x++)
        {
            maze[0, x] = -1;
            maze[mazeHeight - 1, x] = -1;
        }

        // 시작 지점 설정
        startCell = new Vector2Int(Random.Range(1, mazeWidth - 2), Random.Range(1, mazeHeight - 2));
        maze[startCell.y, startCell.x] = 2;

        // DFS 알고리즘을 사용하여 미로 생성
        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        stack.Push(startCell);

        while (stack.Count > 0)
        {
            Vector2Int currentCell = stack.Pop();

            List<Vector2Int> neighbors = GetUnvisitedNeighbors(currentCell);

            if (neighbors.Count > 0)
            {
                stack.Push(currentCell);

                int randomIndex = Random.Range(0, neighbors.Count);
                Vector2Int randomNeighbor = neighbors[randomIndex];

                maze[randomNeighbor.y, randomNeighbor.x] = 1;
                maze[
                    (currentCell.y + randomNeighbor.y) / 2,
                    (currentCell.x + randomNeighbor.x) / 2
                ] = 1;

                stack.Push(randomNeighbor);

                yield return new WaitForSeconds(delay); // 시간 지연
            }
        }

        // 끝나는 지점 설정
        endCell = GetFarthestCellFromStart();
        maze[endCell.y, endCell.x] = 3;

        DrawMaze();
    }

    List<Vector2Int> GetUnvisitedNeighbors(Vector2Int cell)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        Vector2Int[] offsets = new Vector2Int[]
        {
            Vector2Int.up * 2,
            Vector2Int.down * 2,
            Vector2Int.left * 2,
            Vector2Int.right * 2
        };

        foreach (var offset in offsets)
        {
            Vector2Int neighbor = cell + offset;

            if (IsValidCell(neighbor) && maze[neighbor.y, neighbor.x] == 0)
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    bool IsValidCell(Vector2Int cell)
    {
        return cell.x >= 0 && cell.x < mazeWidth && cell.y >= 0 && cell.y < mazeHeight;
    }

    Vector2Int GetFarthestCellFromStart()
    {
        Vector2Int farthestCell = Vector2Int.zero;
        int maxDistance = 0;

        for (int y = 0; y < mazeHeight; y++)
        {
            for (int x = 0; x < mazeWidth; x++)
            {
                if (maze[y, x] == 0)
                {
                    Vector2Int currentCell = new Vector2Int(x, y);
                    int distance =
                        Mathf.Abs(currentCell.x - startCell.x)
                        + Mathf.Abs(currentCell.y - startCell.y);

                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        farthestCell = currentCell;
                    }
                }
            }
        }

        return farthestCell;
    }

    void DrawMaze()
    {
        // 기존의 미로 오브젝트 삭제
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // 새로운 미로 생성
        for (int y = 0; y < mazeHeight; y++)
        {
            for (int x = 0; x < mazeWidth; x++)
            {
                if (maze[y, x] == 1)
                {
                    Vector3 position = new Vector3(x * wallSize, y * wallSize, 0f);
                    Instantiate(wallPrefab, position, Quaternion.identity, transform);
                }
                else if (maze[y, x] == 2)
                {
                    Vector3 position = new Vector3(x * wallSize, y * wallSize, 0f);
                    Instantiate(startPrefab, position, Quaternion.identity, transform);
                }
                else if (maze[y, x] == 3)
                {
                    Vector3 position = new Vector3(x * wallSize, y * wallSize, 0f);
                    Instantiate(endPrefab, position, Quaternion.identity, transform);
                }
                else if (maze[y, x] == -1)
                {
                    Vector3 position = new Vector3(x * wallSize, y * wallSize, 0f);
                    Instantiate(outerWallPrefab, position, Quaternion.identity, transform);
                }
            }
        }
    }
}
