using System.Collections.Generic;
using Logic.Actions;

namespace Logic.Physics
{	
	public interface IPhysics
	{
		List<IDiceAction> Apply(Grid.Grid grid);		
	}
}
