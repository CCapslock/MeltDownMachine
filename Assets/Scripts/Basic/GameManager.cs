using UnityEngine;

namespace Mans
{

    internal sealed class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            LoadResources.Init();
            ListControllers.Init();
            ListControllers.Add(new GlobalGameController());
        }

        private void Start()
        {
            ListControllers.Initialization();
        }

        private void Update()
        {
            ListControllers.Execute(Time.deltaTime);
        }

        private void LateUpdate()
        {
            ListControllers.LateExecute(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            ListControllers.FixedExecute(Time.deltaTime);
        }
    }
}