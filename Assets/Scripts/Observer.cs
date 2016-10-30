using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IObserver {
    void OnNotification(object sender, Messages subject, params object[] args);
}
public static class MessageSystem {
    private static Dictionary<Messages, List<IObserver>> listeners;
    public static void InitTable() {
        listeners = new Dictionary<Messages, List<IObserver>>();
    }
    public static void Notify(this object sender, Messages subject, params object[] args) {
        if (listeners.ContainsKey(subject))
            foreach (IObserver observer in listeners[subject])
                observer.OnNotification(sender, subject, args);
    }
    public static void RegisterTo(this IObserver observer, Messages subject) {
        if (!listeners.ContainsKey(subject)) {
            listeners.Add(subject, new List<IObserver>() { observer });
        } else {
            if (!listeners[subject].Contains(observer))
                listeners[subject].Add(observer);
        }
    }
    public static void UnregisterTo(this IObserver observer, Messages subject) {
        if (listeners.ContainsKey(subject))
            listeners[subject].Remove(observer);
    }
}