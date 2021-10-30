using UnityEngine;

namespace Mans
{
    public static class Reference
    {
        internal static Camera MainCamera => _mainCamera != null ? _mainCamera : _mainCamera = Camera.main;        
        private static Camera _mainCamera;

        internal static Transform ActiveElements => _trash != null ? _trash : _trash = GameObject.FindObjectOfType<TagFolderActiveElements>().transform;        
        private static Transform _trash;

        internal static Transform Canvas => _canvas != null ? _canvas : _canvas = GameObject.FindObjectOfType<TagCanvas>().transform;
        private static Transform _canvas;

        internal static Transform AudioListener => _audioListener != null ? _audioListener : _audioListener = GameObject.FindObjectOfType<AudioListener>().transform;
        private static Transform _audioListener;

        internal static IReadOnlyGameModel GameModel => _gameModel;
        private static IReadOnlyGameModel _gameModel;
        internal static void AddGameModel(GameModel gameModel) => _gameModel = gameModel;

    }
}