using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum WorldObject
    {
        Unknown,
        Player,
        Monster,
		Crystal,
    }

	public enum State
	{
		Die,
		Moving,
		Idle,
		Skill,
	}

    public enum Layer
    {
        Monster = 8,
        Ground = 9,
        Block = 6,
    }

    public enum Scene
    {
        Unknown,
        Main,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }

    public enum MouseEvent
    {
        Press,
        PointerDown,
        PointerUp,
        Click,
    }

    public enum CameraMode
    {
        QuarterView,
    }
}
