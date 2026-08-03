using UnityEngine;

public class scientist : MonoBehaviour
{
    public enum Stato {escaping, catching, possessed};
    public Stato StatoAttuale;
    public int HitPoints=60;
    void Start()
    {
        if (parassita.StatoAttuale==parassita.Stato.possessing) 
	{	
		this.StatoAttuale=Stato.escaping;
	} else {
		this.StatoAttuale=Stato.catching;
	}
    }
    
    void Update()
    {
        if (this.HitPoints<=0) 
	{
		yield return new WaitForSeconds(1.5f);
		Destroy(this.gameObject);
	}
	switch (StatoAttuale)
	{
		case Stato. catching:
			
			if ('colpiti dal parassita')
			{
				this.StatoAttuale=Stato.possessed;
			}
			if (parassita.StatoAttuale==parassita.Stato.possessing && this.StatoAttuale!=Stato.possessed)
			{
				this.StatoAttuale=Stato.escaping;
			}	
			break;
		case Stato.escaping:
			
			if (parassita.StatoAttuale!=parassita.Stato.possessing)
			{
				this.StatoAttuale=Stato.catching;
			}
			break;
		case Stato.possessed:
			
			break:
	}
    }
}









