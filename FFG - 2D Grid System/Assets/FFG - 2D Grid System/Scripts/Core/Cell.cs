using UnityEngine;


namespace FFG
{
    [System.Serializable]
    public class Cell
    {
        #region Properties

        public bool IsValid { get; set; }
        public object Data { get; set; }
        public Vector2Int Index { get; set; }

        #endregion
        #region Public Methods

        /// <summary>
        /// Override method to format the values inside the cell
        /// </summary>
        public override string ToString() => (Data == null ? "-" : Data.ToString()) + ":" + IsValid;

        #endregion
    }
}