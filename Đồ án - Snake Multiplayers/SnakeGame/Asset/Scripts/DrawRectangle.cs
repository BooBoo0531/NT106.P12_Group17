using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
    public class DrawRectangle : MonoBehaviour
    {
        private void Start()
        {
            
        }

        private void Update()
        {
            
            
        }

        private void OnDrawGizmos()
        {
            //if(GameHandler.Instance == null)
            //{
            //    return;
            //}
            //var levelGrid = GameHandler.Instance.LevelGrid;
            //if (levelGrid != null)
            //{
            //    // Set Gizmo color
            //    Gizmos.color = Color.red;
            //    var levelGridPos = levelGrid.GetLevelGridPosition();
            //    var width = levelGrid.GetWidth();
            //    var height = levelGrid.GetHeight();
            //    // Calculate rectangle corners
            //    Vector3 bottomLeft = new Vector3((levelGridPos.x - width) / 2 + levelGridPos.x, (levelGridPos.y - height) / 2 + levelGridPos.y, 0);
            //    Vector3 bottomRight = new Vector3((levelGridPos.x + width) / 2, (levelGridPos.y - height) / 2 + levelGridPos.y, 0);
            //    Vector3 topLeft = new Vector3((levelGridPos.x - width) / 2 + levelGridPos.x, (levelGridPos.y + height) / 2, 0);
            //    Vector3 topRight = new Vector3((levelGridPos.x + width) / 2, (levelGridPos.y + height) / 2, 0);
            //    //Debug.Log($"Bottom: {bottomLeft.x} : {bottomLeft.y} : {bottomRight.x} : {bottomRight.y}");
            //    //Debug.Log($"Top: {topLeft.x} : {topLeft.y} : {topRight.x} : {topRight.y}");
            //    // Draw lines between corners
            //    Gizmos.DrawLine(topRight, topLeft);
            //    Gizmos.DrawLine(topLeft, bottomLeft);
            //    Gizmos.DrawLine(bottomLeft, bottomRight);
            //    Gizmos.DrawLine(bottomRight, topRight);
            //}
        }
    }
}
