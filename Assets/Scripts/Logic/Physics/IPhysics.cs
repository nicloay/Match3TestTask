using System.Collections.Generic;

namespace Logic.Physics
{	
	public interface IPhysics
	{
		List<DiceMovement> Apply(Grid.Grid grid);		
	}
}
