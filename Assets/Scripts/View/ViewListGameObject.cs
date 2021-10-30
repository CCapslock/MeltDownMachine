using UnityEngine;

namespace Mans
{
    public class ViewListGameObject : MonoBehaviour, IListGameObject
    {
        private const string _nameIconObject = "iconObject";

        public GameObject[] ListGameObjects => _gameObjects;
        [SerializeField] private GameObject[] _gameObjects;

        private void OnDrawGizmosSelected()
        {
            if (_gameObjects == null) return;

            for (int i = 0; i < _gameObjects.Length; i++)
            {
                if (_gameObjects[i]!=null) Gizmos.DrawIcon(_gameObjects[i].transform.position, _nameIconObject,true);
            }
        }
    }   
}