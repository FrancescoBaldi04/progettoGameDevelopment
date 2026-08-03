using UnityEngine;

public class turret : MonoBehaviour
{
    public enum Stato {waiting, shooting};
    public Stato StatoAttuale;
    void Start()
    {
        if (parassita.StatoAttuale==parassita.Stato.possessing) 
	{	
		this.StatoAttuale=Stato.shooting;
	} else {
		this.StatoAttuale=Stato.waiting;
	}
    }
    
    void Update()
    {
	switch (StatoAttuale)
	{
		case Stato.waiting:
			if (parassita.StatoAttuale==parassita.Stato.possessing)
			{
				this.StatoAttuale=Stato.shooting;
			}	
			break;
		case Stato.shooting:
			
			if (parassita.StatoAttuale!=parassita.Stato.possessing)
			{
				this.StatoAttuale=Stato.waiting;
			}
			break;
	}
    }
}










