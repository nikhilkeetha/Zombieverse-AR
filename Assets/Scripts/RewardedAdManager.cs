using GoogleMobileAds.Api;
using UnityEngine;
using TMPro;

public class RewardedAdManager : MonoBehaviour
{
    private RewardedAd rewardedAd;
    public bool isAdLoaded=false;

    // Start is called before the first frame update
    public void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
            
        });
        LoadRewardedAd();
        
    }
#if UNITY_ANDROID
  // Google's public test ad unit ID - replace with your own AdMob ad unit ID before publishing.
  private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
  private string _adUnitId = "unused";
#endif

  /// <summary>
  /// Loads the rewarded ad.
  /// </summary>
  public void LoadRewardedAd()
  {
      // Clean up the old ad before loading a new one.
      if (rewardedAd != null)
      {
            rewardedAd.Destroy();
            rewardedAd = null;
      }

      Debug.Log("Loading the rewarded ad.");

      // create our request used to load the ad.
      var adRequest = new AdRequest.Builder().Build();

      // send the request to load the ad.
      RewardedAd.Load(_adUnitId, adRequest,
          (RewardedAd ad, LoadAdError error) =>
          {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
              {
                  Debug.LogError("Rewarded ad failed to load an ad " +
                                 "with error : " + error);
                  return;
              }

              Debug.Log("Rewarded ad loaded with response : "
                        + ad.GetResponseInfo());

              rewardedAd = ad;
              isAdLoaded=true;
          });
          //rewardedAd.OnAdImpressionRecorded += RegisterEventHandlers;
  }

    public void ShowRewardedAd()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                PlayerHealth playerHealth = GameObject.Find("zombieDestination").GetComponent<PlayerHealth>();
                playerHealth.rewardResume();

                LoadRewardedAd();
                
            });
        }
    }

}
