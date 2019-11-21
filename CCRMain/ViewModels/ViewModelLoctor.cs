using JLib.Singleton;
using System;

namespace CCRMain.ViewModels
{
    public static class ViewModelLoctor
    {
        public static CallNumberViewModel CallNumberViewModel = Singleton<CallNumberViewModel>.Instance;
    }
}