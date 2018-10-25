using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    float animstarttime;
    float duration = 14f;

    int numberOfIMGs = 116;
    int numOfAudio = 31;
    string obj_path;

    private bool loadScene = false;
    //public AudioClip aud;
    [SerializeField]
    private int scene;
    [SerializeField]
    //private Text loadingText;

    // Updates once per frame
    void Update()
    {

        // If the player has pressed the space bar and a new scene is not loading yet...
        if (Input.GetMouseButtonUp(0) && loadScene == false)
        {
            animstarttime = Time.time;
            // ...set the loadScene boolean to true to prevent loading a new scene more than once...
            loadScene = true;


            //// ...change the instruction text to read "Loading..."
            //loadingText.text = "Loading...";

            // ...and start a coroutine that will load the desired scene.
            //StartCoroutine(LoadNewScene());

            //LoadObjectArray();
            StartCoroutine(testAssetAsync());


        }

        // If the new scene has started loading...
        if (loadScene == true)
        {

            var c = Camera.main.transform;
            var currentPos = c.position;
            var timer = (Time.time - animstarttime) / duration;
            c.position = new Vector3(currentPos.x, Mathf.SmoothStep(currentPos.y, -90f, timer), currentPos.z); //Vector3.Lerp(currentPos, new Vector3(0f, -90f, -10f), timer);

            //// ...then pulse the transparency of the loading text to let the player know that the computer is still working.
            //loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));

        }

    }

    IEnumerator testAssetAsync()
    {
        GameObject assetHolder = new GameObject("assetHolder");
        var alh = assetHolder.AddComponent<AssetLoaderHolder>();

        for (int i = 0; i < numOfAudio; i++)
        {
            obj_path = "Audio/" + (i + 1);

            var RuntimeLoadedAudioRequest = Resources.LoadAsync(obj_path, typeof(AudioClip));//.ToArray();
                                                                                             //RuntimeLoadedPrefabCRequest = ()
            while (!RuntimeLoadedAudioRequest.isDone)
            {
                print(RuntimeLoadedAudioRequest.progress);
                yield return null;
            }
            AudioClip aud = (AudioClip)RuntimeLoadedAudioRequest.asset;
            alh.audioClips.Add(aud);
        }

        for (int i = 0; i < numberOfIMGs; i++)
        {
            if ((i + 1) < 10)
            {
                obj_path = "ImagesVoyager/00" + (i + 1);
            }
            if ((i + 1) >= 10 && (i + 1) < 100)
            {
                obj_path = "ImagesVoyager/0" + (i + 1);
            }
            if ((i + 1) >= 100)
            {
                obj_path = "ImagesVoyager/" + (i + 1);
            }
            var RuntimeLoadedPicRequest = Resources.LoadAsync(obj_path, typeof(Sprite));
            while (!RuntimeLoadedPicRequest.isDone)
            {
                print(RuntimeLoadedPicRequest.progress);
                yield return null;
            }
            Sprite s = (Sprite)RuntimeLoadedPicRequest.asset;
            alh.sprites.Add(s);
        }

        AsyncOperation async = SceneManager.LoadSceneAsync(scene); // Application.LoadLevelAsync(scene);


        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            print(async.progress);
            yield return null;
        }
        yield return async;

    }

    // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
    IEnumerator LoadNewScene()
    {

        //// This line waits for 3 seconds before executing the next line in the coroutine.
        //// This line is only necessary for this demo. The scenes are so simple that they load too fast to read the "Loading..." text.
        //yield return new WaitForSeconds(3);

        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation async = SceneManager.LoadSceneAsync(scene); // Application.LoadLevelAsync(scene);


        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            print(async.progress);
            yield return null;
        }
        yield return async;

    }

}
