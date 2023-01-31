using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;
using GlobalsNS;

public class mobileNotifications : MonoBehaviour
{
    void Start()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
        AndroidNotificationCenter.CancelAllNotifications();

        var notification1 = new AndroidNotification();
        if (Globals.PITCHTYPE.Equals("INDOOR"))
            notification1.Title = "Goalie Wars Football Indoor";
        else if (Globals.PITCHTYPE.Equals("STREET"))
        {
            notification1.Title = "Goalie Wars Football Street";
        }
        else
        {
            notification1.Title = "Goalie Wars Football Online";
        }

        notification1.Text = "Ready to win the next match?";
        notification1.FireTime = System.DateTime.Now.AddMinutes(60);
        notification1.LargeIcon = "icon_1";

        var notification2 = new AndroidNotification();

        if (Globals.PITCHTYPE.Equals("INDOOR"))
        {
            notification2.Title = "Goalie Wars Football Indoor";
        }
        else if (Globals.PITCHTYPE.Equals("STREET"))
        {
            notification2.Title = "Goalie Wars Football Street";
        }
        else
            notification2.Title = "Goalie Wars Football Online";

        notification2.Text = "Ready to win the next match?";
        notification2.FireTime = System.DateTime.Now.AddDays(1);
        notification2.LargeIcon = "icon_1";

        //5 days
        var notification3 = new AndroidNotification();

        if (Globals.PITCHTYPE.Equals("INDOOR"))
            notification3.Title = "Goalie Wars Football Indoor";
        else if (Globals.PITCHTYPE.Equals("STREET"))
        {
            notification3.Title = "Goalie Wars Football Street";
        }
        else
            notification3.Title = "Goalie Wars Football Online";

        notification3.Text = "Ready to win the next match?";
        notification3.FireTime = System.DateTime.Now.AddDays(5);
        notification3.LargeIcon = "icon_1";

        //10 days
        var notification4 = new AndroidNotification();
        if (Globals.PITCHTYPE.Equals("INDOOR"))
            notification4.Title = "Goalie Wars Football Indoor";
        else if (Globals.PITCHTYPE.Equals("STREET"))
        {
            notification4.Title = "Goalie Wars Football Street";
        }
        else
            notification4.Title = "Goalie Wars Football Online";

        notification4.Text = "Ready to win the next match?";
        notification4.FireTime = System.DateTime.Now.AddDays(10);
        notification4.LargeIcon = "icon_1";

        //20 days
        var notification5 = new AndroidNotification();
        if (Globals.PITCHTYPE.Equals("INDOOR"))
            notification5.Title = "Goalie Wars Football Indoor";
        else if (Globals.PITCHTYPE.Equals("STREET"))
        {
            notification5.Title = "Goalie Wars Football Street";
        }
        else
            notification5.Title = "Goalie Wars Football Online";

        notification5.Text = "Ready to win the next match?";
        notification5.FireTime = System.DateTime.Now.AddDays(20);
        notification5.LargeIcon = "icon_1";

        //40 days
        var notification6 = new AndroidNotification();

        if (Globals.PITCHTYPE.Equals("INDOOR"))
            notification6.Title = "Goalie Wars Football Indoor";
        else if (Globals.PITCHTYPE.Equals("STREET"))
        {
            notification6.Title = "Goalie Wars Football Street";
        }
        else
            notification6.Title = "Goalie Wars Football Online";

        notification6.Text = "Ready to win the next match?";
        notification6.FireTime = System.DateTime.Now.AddDays(40);
        notification6.LargeIcon = "icon_1";

        var notification7 = new AndroidNotification();

        if (Globals.PITCHTYPE.Equals("INDOOR"))
            notification7.Title = "Goalie Wars Football Indoor";
        else if (Globals.PITCHTYPE.Equals("STREET"))
        {
            notification7.Title = "Goalie Wars Football Street";
        }
        else
            notification7.Title = "Goalie Wars Football Online";

        notification7.Text = "Ready to win the next match?";
        notification7.FireTime = System.DateTime.Now.AddDays(80);
        notification7.LargeIcon = "icon_1";

        AndroidNotificationCenter.SendNotification(notification1, "channel_id");
        AndroidNotificationCenter.SendNotification(notification2, "channel_id");
        AndroidNotificationCenter.SendNotification(notification3, "channel_id");
        AndroidNotificationCenter.SendNotification(notification4, "channel_id");
        AndroidNotificationCenter.SendNotification(notification5, "channel_id");
        AndroidNotificationCenter.SendNotification(notification6, "channel_id");
        AndroidNotificationCenter.SendNotification(notification7, "channel_id");    
    }
}

