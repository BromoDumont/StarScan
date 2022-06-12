using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class ADsManager : MonoBehaviour, IUnityAdsListener
{
#if UNITY_IOS
    string gameId = "4795226";
#else
    string gameId = "4795227";
#endif

    [Tooltip ("Conecta o script 'GameManager' com o script 'ADsManager'.")] [SerializeField] 
    private GameManager _GameManager;

    void Start()
    {
        Advertisement.Initialize(gameId);
        Advertisement.AddListener(this);
    }

    public void NewLifeAd()
    {
         if (Advertisement.IsReady("Rewarded_Android"))
         {
            Advertisement.Show("Rewarded_Android");
         }
         else
         {
            Debug.Log("Reward ad is not ready!");
         }
    }

    public void OnUnityAdsReady(string placementId)
    {
        Debug.Log(placementId + "Ad is ready!");
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log(placementId + " Ad is started!");
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId == "Rewarded_Android" && showResult == ShowResult.Finished)
        {
            _GameManager.GameRestart(true);
            Debug.Log("Reward ad is watched!");
        }
    }

        public void OnUnityAdsDidError(string message)
    {
        Debug.Log("Error: " + message);
    }
}
