using UnityEngine;

public class UiManager : MonoBehaviour
{

	enum UiState
	{
		None,
		LobbyPanel,

	}

	
	//Panel

	public GameObject LobbyPanel;
	

	private UiState state = UiState.None;


	private void Awake()
	{
		state = UiState.LobbyPanel;
		LobbyPanel.SetActive(true);
	}



	public void OnUndoButton()
	{
		return;

		//Debug.Log("뒤로가기 버튼 클릭");
		//switch (state)
		//{
		//	case UiState.None:
		//		Debug.Log("양정우 여기 고쳐줘");
		//		break;

		//	case UiState.SelectPanel:
		//		Debug.Log("SelectPanel 상태");
		//		break;

		//	case UiState.IpPanel:
		//		Debug.Log("IpPanel 상태");
		//		SelectPanel.SetActive(true);
		//		IpPanel.SetActive(false);
		//		state = UiState.SelectPanel;
		//		break;

		//	case UiState.LobbyPanel:
		//		Debug.Log("LobbyPanel 상태");
		//		SelectPanel.SetActive(true);
		//		LobbyPanel.SetActive(false);
		//		state = UiState.SelectPanel;
		//		break;

		//}
	}
}
