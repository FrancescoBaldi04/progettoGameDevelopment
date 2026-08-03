using UnityEngine;

public class guard : MonoBehaviour
{
    public enum Stato {escaping, positioning, shooting, possessed};
    public Stato StatoAttuale;
    public int HitPoints=60;
    void Start()
    {
        if (parassita.StatoAttuale==parassita.Stato.possessing) 
	{	
		this.StatoAttuale=Stato.positioning;
	} else {
		this.StatoAttuale=Stato.escaping;
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
		case Stato.escaping:
			
			if ('colpiti dal parassita')
			{
				this.StatoAttuale=Stato.possessed;
				this.HitPoints=60;
			}
			if (parassita.StatoAttuale==parassita.Stato.possessing && this.StatoAttuale!=Stato.possessed)
			{
				this.StatoAttuale=Stato.positioning;
			}	
			break;
		case Stato.positioning:
			
			if ('raggiunge la posizione')
			{
				this.StatoAttuale=Stato.shooting;
			}
			if (parassita.StatoAttuale!=parassita.Stato.possessing)
			{
				this.StatoAttuale=Stato.escaping;
			}
			break;
		case Stato.shooting:
			
			break;
		case Stato.possessed:
			
			break:
	}
    }
}
