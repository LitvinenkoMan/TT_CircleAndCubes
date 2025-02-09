using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Scripts.Interfaces
{
    public interface IMovable
    {
        UniTask MoveToPosition(Vector2 pos, float moveTime);
        UniTask MoveAlongTheLine(Queue<Vector2> line, float moveTime);
    }
}
