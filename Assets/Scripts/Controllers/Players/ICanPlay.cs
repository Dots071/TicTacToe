using System.Collections.Generic;
using System;

namespace Controllers.Players
{
    /// <summary>
    /// This interface helps us support different types of players.
    /// </summary>
    public interface ICanPlay
    {
        public void CancelTurn();
        public void PlayTurn(Action<int, int> tilePlayedCallback, List<int[]> movesLog);

    }
}

