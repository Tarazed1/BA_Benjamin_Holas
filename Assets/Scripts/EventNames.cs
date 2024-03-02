using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insection
{
    public static class EventNames
    {
        //root
        public const string EVENT_START = "start";
        public const string EVENT_END = "end";
        //forst choice
        public const string EVENT_WATCH_WORKERS = "watch_workers";
        public const string EVENT_JOIN_MEETING = "join_meeting";
        //second choice
        public const string EVENT_GO_TO_FAR_FLOWERS_LESS_KNOWLODGE = "go_to_far_flowers_less_knowledge";
        public const string EVENT_GO_TO_MIDDLE_FLOWERS_LESS_KNOWLEDGE = "go_to_middle_flowers_less_knowledge";
        public const string EVENT_GO_TO_FAR_FLOWERS = "go_to_far_flowers";
        public const string EVENT_GO_TO_MIDDLE_FLOWERS = "go_to_middle_flowers";
        public const string EVENT_GO_TO_CLOSE_FLOWERS = "go_to_close_flowers";
        //third choice
        public const string EVENT_BREAK = "break";
        public const string EVENT_CONTINUE = "continue";
    }
}