using UnityEngine;

public class CallToast : MonoBehaviour {

    // To store context of the main activity that Unity3D is running
    private AndroidJavaObject activityContext = null;

    // To store the instance of the class
    private AndroidJavaObject toastExample = null;

    // Use this for initialization
    void Start () {
        // Get MainActivity class instance that Unity is running
        using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            // Get context of the MainActivity class that unity is running
            activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
        }

        // Get class from the jar
        using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.plugin.android.simpleplugin.PluginClass"))
        {
            if (pluginClass != null)
            {
                // get singelton instance of from the class in object toastExample 
                toastExample = pluginClass.CallStatic<AndroidJavaObject>("instance");

                // pass context to the class
                toastExample.Call("setContext", activityContext);
            }
        }
    }

    public void ShowToast()
    {
        // check whether toastExample object is created or not
        if (toastExample != null)
        {
            // call toast on main thread as toast is will be shown on HUD
            // and every UI elements is called on main thread
            activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                toastExample.Call("showMessage", "This is a Toast message");
            }));
        }
    }
}
