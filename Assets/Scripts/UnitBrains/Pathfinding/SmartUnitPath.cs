using Model;
using System.Collections.Generic;
using System.Linq;
using UnitBrains.Pathfinding;
using UnityEngine;

namespace Assets.Scripts.UnitBrains.Pathfinding
{
    public class SmartUnitPath : BaseUnitPath
    {
        private Vector2Int _startPosition;
        private Vector2Int _targetPosition;
        private int[] dx = { -1, 0, 1, 0 };
        private int[] dy = { 0, 1, 0, -1 };

        private bool _isTarget;
        private bool _isEnemyUnitClose;
        private SmartNode _nextToEnemyUnit;

        public SmartUnitPath(IReadOnlyRuntimeModel runtimeModel, Vector2Int startPosition, Vector2Int targetPosition) :
            base(runtimeModel, startPosition, targetPosition)
        {
            _startPosition = startPosition;
            _targetPosition = targetPosition;
        }

        protected override void Calculate()
        {

            SmartNode startNode = new SmartNode(_startPosition);// Çàäà¸ò ñòàðòîâûå êîîðäèíàòû
            SmartNode targetNode = new SmartNode(_targetPosition);// Çàäà¸ò êîîðäèíàòû öåëè
            List<SmartNode> openList = new List<SmartNode> { startNode };// Â ñïèñîê âíîñÿòñÿ âåðøèíû â êîòîðûå ìîæíî ïîéòè
            List<SmartNode> closedList = new List<SmartNode>();// Â ñïèñîê âíîñÿòñÿ ïðîéäåííûå âåðøèíû, êîòîðûå íå ó÷àñòâóþò â âû÷èñëåíèÿõ

            int counter = 0;
            int maxCount = runtimeModel.RoMap.Width * runtimeModel.RoMap.Height;// Îãðàíè÷èâàåò ìàêñèìóì øàãîâ ðàçìåðàìè êàðòû

            while (openList.Count > 0 && counter++ < maxCount)// Öèêë âûïîëíÿåòñÿ ïîêà â openList åù¸ åñòü íîäû
            {
                SmartNode currentNode = openList[0];// Âûáèðàåòñÿ ïåðâàÿ íîäà èç ñïèñêà (èíäåêñàöèÿ íà÷èíàåòñÿ ñ 0)

                foreach (var node in openList)
                {
                    if (node.Value < currentNode.Value)// Ïåðåáèðàåò íîäû â ñïèñêå è èùåò ñ íàèìåíüøèì çíà÷åíèåì ýâðèñòè÷åñêîé ôóíêöèè
                        currentNode = node;// Äåëàåò òàêóþ íîäó òåêóùåé
                }

                openList.Remove(currentNode);//Ðàç ýòà íîäà ïðîéäåíà, òî îíà èñêëþ÷àåòñÿ èç îòêðûòîãî ñïèñêà
                closedList.Add(currentNode);// È çà÷èñëÿåòñÿ â çàêðûòûé

                if (_isTarget)
                {
                    path = FindPath(currentNode);
                    return;
                }

                for (int i = 0; i < dx.Length; i++)
                {
                    // Ñêëàäûâàþò êîîðäèíàòó òåêóùåé íîäû è å¸ ñìåùåíèå ïî îñè, è âûäàåò íîâóþ êîîðäèíàòó ïî Õ è Y ñîîòâåòñòâåííî
                    int newX = currentNode.Position.x + dx[i];
                    int newY = currentNode.Position.y + dy[i];
                    var newPosition = new Vector2Int(newX, newY);//Íîâàÿ ïîçèöèÿ 

                    if (newPosition == targetNode.Position)// Ïðîâåðÿåò, áóäåò ëè ñëåäóþùàÿ ïîçèöèÿ öåëåâîé
                        _isTarget = true;

                    if (IsValid(newPosition) || _isTarget)// Åñëè êëåòêà äîñòóïíà äëÿ õîäà è îíà ÿâëÿåòñÿ öåëåâîé
                    {
                        SmartNode neighbor = new SmartNode(newPosition);// Äëÿ íå¸ ñîçäà¸òñÿ íîäà

                        if (closedList.Contains(neighbor))// Ïðîâåðÿåì, ÷òî ýòîé íîäû íåò â çàêðûòîì ñïèñêå
                            continue;

                        neighbor.Parent = currentNode;// Óêàçûâàåì â íàïðàâëåíèè òåêóùóþ íîäó
                        neighbor.CalculateEstimate(targetNode.Position);// Ðàññ÷èòûâàåì ðàññòîÿíèå
                        neighbor.CalculateValue();// È ñòîèìîñòü ýâðèñòè÷åñêîé ôóíêöèè

                        openList.Add(neighbor);// Äîáàâëÿåì íîäó â îòêðûòûé ñïèñîê
                    }
                    if (CheckCollisionWithEnemy(newPosition) && !_isEnemyUnitClose)
                    {
                        _isEnemyUnitClose = true;
                        _nextToEnemyUnit = currentNode;
                    }
                }
            }
            if (_isEnemyUnitClose)
            {
                path = FindPath(_nextToEnemyUnit);
                return;
            }

            path = new Vector2Int[] { startNode.Position };
        }

        private Vector2Int[] FindPath(SmartNode currentNode)
        {
            List<Vector2Int> path = new();

            while (currentNode != null)// Öèêë äâèæåòñÿ â îáðàòíîì ïîðÿäêå, ïîêà currentNode èìååò çíà÷åíèå
            {
                path.Add(currentNode.Position);// Ïîìåùàåò òåêóùóþ íîäó â ñïèñîê "ïóòü"
                currentNode = currentNode.Parent;// Ïîäñòàâëÿåò ïîä òåêóùóþ íîäó ñëåäóþùóþ èç Parent
            }

            path.Reverse();// Ðàçâîðà÷èâàåò ñïèñîê â îáðàòíîì (ïðàâèëüíîì) ïîðÿäêå
            return path.ToArray();// Âîçâðàùàåò ñïèñîê ïóòè íà÷èíàÿ ñ êîîðäèíàòû âðàãà
        }

        private bool IsValid(Vector2Int point)//Ïðîâåðÿåò ïðîõîäèìîñòü êëåòêè íà êàðòå
        {
            return runtimeModel.IsTileWalkable(point);
        }

        private bool CheckCollisionWithEnemy(Vector2Int newPos)//Ïðîâåðÿåò ñòîëêíîâåíèå ñ âðàãîì
        {
            var botUnitPositions = runtimeModel.RoBotUnits.Select(u => u.Pos).Where(u => u == newPos);

            return botUnitPositions.Any();
        }
    }
}