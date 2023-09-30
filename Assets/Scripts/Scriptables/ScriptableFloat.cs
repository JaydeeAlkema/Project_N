using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Float", menuName = "ScriptableObjects/New Scriptable Float")]
public class ScriptableFloat : ScriptableObject
{
	public float value;
	public bool resetOnDestroy = false;
	[ShowIf("resetOnDestroy")] public float startValue;

	private void OnDestroy()
	{
		Reset();
	}

	private void OnEnable()
	{
		Reset();
	}

	private void OnDisable()
	{
		Reset();
	}

	public void Reset()
	{
		value = startValue;
	}
}
