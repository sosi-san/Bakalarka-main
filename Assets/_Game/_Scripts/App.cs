using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Woska.Core;

namespace Woska.Bakalarka
{
    public class App : Singleton<App>
    {
        public NetworkController NetworkController { get; private set; }
        public GameController GameManager { get; private set; }

        private void Awake()
        {
            FindBaseScripts();
        }
        private void FindBaseScripts()
        {
            NetworkController = gameObject.GetComponent<NetworkController>();
            GameManager = gameObject.GetComponent<GameController>();
        }
        public void QuitGame()
        {
            AppHelper.Quit();
        }
    }
    public enum SceneIndexes
    {
        MAIN,
        SPIKY_WHEEL,
        JUMP_ROPE,
        DEATH_RUN,

        NUMBER_OF_SCENES
    }
}