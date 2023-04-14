using UnityEngine;
using UnityEngine.UI;

namespace SHamilton.ClubParty.UI {
    public class FlexibleGridLayout : LayoutGroup {
    
        public enum FitType {
            Uniform,
            Width,
            Height,
            FixedRows,
            FixedColumns,
        }

        public FitType fitType;
        public int rows;
        public int columns;
        public Vector2 cellSize;
        public Vector2 spacing;
        public bool fitX;
        public bool fitY;

        public override void CalculateLayoutInputHorizontal() {
            base.CalculateLayoutInputHorizontal();
            
            if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform) {
                var sqrtChildCount = Mathf.Sqrt(transform.childCount);
                rows = Mathf.CeilToInt(sqrtChildCount);
                columns = Mathf.CeilToInt(sqrtChildCount);
            }

            switch (fitType) {
                case FitType.Width:
                case FitType.FixedColumns:
                    rows = Mathf.CeilToInt(transform.childCount / (float)columns);
                    break;
                case FitType.Height:
                case FitType.FixedRows:
                    columns = Mathf.CeilToInt(transform.childCount / (float)rows);
                    break;
            }

            var parentWidth = rectTransform.rect.width;
            var parentHeight = rectTransform.rect.height;
            
            var cellWidth = (parentWidth / columns) - ((spacing.x / columns) * 2) - (padding.left / (float)columns) - (padding.right / (float)columns);
            var cellHeight = (parentHeight / rows) - ((spacing.y / rows) * 2) - (padding.top / (float)rows) - (padding.bottom / (float)rows);

            cellSize.x = fitX ? cellWidth : cellSize.x;
            cellSize.y = fitY ? cellHeight : cellSize.y;

            for (int i = 0; i < rectChildren.Count; i++) {
                var currentRow = i / columns;
                var currentColumn = i % columns;

                var child = rectChildren[i];

                var xPos = (cellSize.x * currentColumn) + (spacing.x * currentColumn) + padding.left;
                var yPos = (cellSize.y * currentRow) + (spacing.y * currentRow) + padding.top;

                SetChildAlongAxis(child, 0, xPos, cellSize.x);
                SetChildAlongAxis(child, 1, yPos, cellSize.y);
            }
        }

        public override void CalculateLayoutInputVertical() {
            
        }

        public override void SetLayoutHorizontal() {
            
        }

        public override void SetLayoutVertical() {
            
        }
    }
}

