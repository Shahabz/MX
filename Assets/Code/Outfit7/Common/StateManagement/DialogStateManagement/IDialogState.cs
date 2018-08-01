﻿//
//   Copyright (c) 2014 Outfit7. All rights reserved.
//

namespace Outfit7.StateManagement.DialogStateManagement {
    public interface IDialogState : IAutoOpenState {

        bool BlockOtherDialogs(IDialogState dialogState);

    }
}

