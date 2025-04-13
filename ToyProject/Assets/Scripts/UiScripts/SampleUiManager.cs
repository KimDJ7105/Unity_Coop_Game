using UnityEngine;

public class SmapleUiManager : MonoBehaviour
{

	enum UiState
	{
		None,
		SamplePanel,

    }

	
	//Panel
	public GameObject SamplePanel;


    private UiState state = UiState.None;


	private void Awake()
	{
		state = UiState.SamplePanel;
        SamplePanel.SetActive(true);
	}

    public void OnButtonNumberOne()
	{
        Debug.Log("1번 눌렸다");
    }
    public void OnButtonNumberTwo()
    {
        Debug.Log("2번 눌렸다");
    }
}
