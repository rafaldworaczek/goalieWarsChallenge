using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Play.AppUpdate;
using Google.Play.Common;
using GlobalsNS;

public class AndroidUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    AppUpdateManager appUpdateManager;
    void Start()
    {
        #if UNITY_EDITOR
            return;
        #endif

        Globals.isMultiplayerUpdate = false;
        if (Globals.isMultiplayer)
        {
            appUpdateManager = new AppUpdateManager();
            StartCoroutine(CheckForUpdate());
        }
    }
 
    IEnumerator CheckForUpdate()
    {
        PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoOperation =
          appUpdateManager.GetAppUpdateInfo();

        // Wait until the asynchronous operation completes.
        yield return appUpdateInfoOperation;

        if (appUpdateInfoOperation.IsSuccessful)
        {
            var appUpdateInfoResult = appUpdateInfoOperation.GetResult();
            // Check AppUpdateInfo's UpdateAvailability, UpdatePriority,
            // IsUpdateTypeAllowed(), etc. and decide whether to ask the user
            // to start an in-app update.
            if (appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateAvailable)
            {
                Globals.isMultiplayerUpdate = true;
                var appUpdateOptions = AppUpdateOptions.ImmediateAppUpdateOptions();
                StartCoroutine(StartImmediateUpdate(appUpdateInfoResult, appUpdateOptions));
            } 
        }
        else
        {
            Globals.isMultiplayerUpdate = false;
            // Log appUpdateInfoOperation.Error.
        }

    }

    IEnumerator StartImmediateUpdate(AppUpdateInfo appUpdateInfoResult, 
                                     AppUpdateOptions appUpdateOptions)
    {
        // Creates an AppUpdateRequest that can be used to monitor the
        // requested in-app update flow.
        var startUpdateRequest = appUpdateManager.StartUpdate(
          // The result returned by PlayAsyncOperation.GetResult().
          appUpdateInfoResult,
          // The AppUpdateOptions created defining the requested in-app update
          // and its parameters.
          appUpdateOptions);
        yield return startUpdateRequest;

        // If the update completes successfully, then the app restarts and this line
        // is never reached. If this line is reached, then handle the failure (for
        // example, by logging result.Error or by displaying a message to the user).
    }
   
}
