using System.Collections.Generic;

public sealed class SubscriptionHandler
{
    private readonly Dictionary<string, SubscriptionData> ActiveSubs = new();

    public void CreateSubscription(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return;
        if (ActiveSubs.ContainsKey(name)) return;

        ActiveSubs[name] = new SubscriptionData();
    }

    public void Subscribe(string subscription, string subscriber)
    {
        if (string.IsNullOrWhiteSpace(subscription)) return;
        if (string.IsNullOrWhiteSpace(subscriber)) return;

        if (!ActiveSubs.TryGetValue(subscription, out var sub))
        {
            sub = new SubscriptionData();
            ActiveSubs[subscription] = sub;
        }

        if (!sub.CursorBySubscriber.ContainsKey(subscriber))
        {
            // Start reading from "now" (ignore old history) by default.
            // If you want new subscribers to receive full history, set this to 0 instead.
            sub.CursorBySubscriber[subscriber] = sub.Changelog.Count;
        }
    }

    public Change Fetch(string subscription, string subscriber)
    {
        if (string.IsNullOrWhiteSpace(subscription)) return new Change { kind = 0 };
        if (string.IsNullOrWhiteSpace(subscriber)) return new Change { kind = 0 };
        if (!ActiveSubs.TryGetValue(subscription, out var sub)) return new Change { kind = 0 };

        if (!sub.CursorBySubscriber.TryGetValue(subscriber, out var cursor))
        {
            // Auto-subscribe if not subscribed yet.
            cursor = sub.Changelog.Count;
            sub.CursorBySubscriber[subscriber] = cursor;
            return new Change { kind = 0 };
        }

        if (cursor >= sub.Changelog.Count)
        {
            // Nothing new.
            return new Change { kind = 0 };
        }

        var change = sub.Changelog[cursor];
        sub.CursorBySubscriber[subscriber] = cursor + 1;

        // Cleanup: remove log entries that ALL subscribers have already consumed.
        Cleanup(sub);

        return change;
    }

    public void Log(string subscription, Change change)
    {
        if (string.IsNullOrWhiteSpace(subscription)) return;

        if (!ActiveSubs.TryGetValue(subscription, out var sub))
        {
            sub = new SubscriptionData();
            ActiveSubs[subscription] = sub;
        }

        sub.Changelog.Add(change);

        // Optional periodic cleanup guard. Cheap and keeps memory bounded.
        // Only run if we have a bit of history.
        if (sub.Changelog.Count > 1024)
        {
            Cleanup(sub);
        }
    }

    private static void Cleanup(SubscriptionData sub)
    {
        if (sub.CursorBySubscriber.Count == 0) return;
        if (sub.Changelog.Count == 0) return;

        int minCursor = int.MaxValue;
        foreach (var kv in sub.CursorBySubscriber)
        {
            if (kv.Value < minCursor) minCursor = kv.Value;
        }

        if (minCursor <= 0) return;

        // Trim everything that all subscribers have already read.
        // RemoveRange shifts the list; for structural changes this is fine.
        if (minCursor > sub.Changelog.Count) minCursor = sub.Changelog.Count;
        sub.Changelog.RemoveRange(0, minCursor);

        // Adjust subscriber cursors after trimming.
        var keys = new List<string>(sub.CursorBySubscriber.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            var k = keys[i];
            int v = sub.CursorBySubscriber[k] - minCursor;
            sub.CursorBySubscriber[k] = v < 0 ? 0 : v;
        }
    }

}

public struct Change
{
    public int kind;   // z.B. 1=BuildingPlaced, 2=GridRectSet
    public int a, b, c, d;
    public string descr; // optional nur debug
}

public sealed class SubscriptionData
{
    public readonly List<Change> Changelog = new();
    public readonly Dictionary<string, int> CursorBySubscriber = new(); // subscriber -> next index
}