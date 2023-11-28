
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace jp.co.jetman.common.graph
{
    [RequireComponent(typeof(Image), typeof(RectTransform))]
    public class GraphMarkerBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Sprite[] _sprites;
        private Image image;
        private RectTransform rect;

        #region MonoBehaviour
        void Awake()
        {
            image = GetComponent<Image>();
            rect = GetComponent<RectTransform>();
        }
        #endregion

        #region Public Methods
        public void SetIconByIndex(int _index)
        {
            if (_index < _sprites.Length)
            {
                image.sprite = _sprites[_index];
            }
        }
        public void SetPosition(Vector2 _position)
        {
            rect.anchoredPosition = _position;
        }
        public void Destroy()
        {
            GameObject.Destroy(gameObject);
        }
        #endregion
    }
}