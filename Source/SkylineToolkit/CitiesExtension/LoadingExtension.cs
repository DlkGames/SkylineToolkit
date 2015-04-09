using ICities;

namespace SkylineToolkit.CitiesExtension
{
    public abstract class LoadingExtension : ICitiesLoadingExtension, ICitiesExtension
    {
        public static readonly string Theme_Sunny = "Sunny";
        public static readonly string Theme_North = "North";
        public static readonly string Theme_Tropical = "Tropical";

        private bool isCreated;

        public bool IsCreated
        {
            get { return isCreated; }
            private set { isCreated = value; }
        }

        public IManagers Managers
        {
            get { return this.LoadingManager.managers; }
        }

        public ILoading LoadingManager { get; private set; }

        public string CurrentTheme
        {
            get
            {
                return this.LoadingManager.currentTheme;
            }
            set
            {
                this.LoadingManager.currentTheme = value;
            }
        }

        public bool LoadingComplete
        {
            get { return this.LoadingManager.loadingComplete; }
        }

        public AppMode CurrentMode
        {
            get { return this.LoadingManager.currentMode; }
        }

        public virtual void OnCreated(ILoading loading)
        {
            this.LoadingManager = loading;
            this.isCreated = true;
        }

        public virtual void OnLevelLoaded(LoadMode mode)
        {
            OnGameStarted();

            switch (mode)
            {
                case LoadMode.LoadAsset:
                    OnAssetLoaded();
                    break;
                case LoadMode.LoadGame:
                    OnGameLoaded();
                    break;
                case LoadMode.LoadMap:
                    OnMapLoaded();
                    break;
                case LoadMode.NewAsset:
                    OnNewAssetStarted();
                    break;
                case LoadMode.NewGame:
                    OnNewGameStarted();
                    break;
                case LoadMode.NewMap:
                    OnNewMapStarted();
                    break;
            }
        }

        public virtual void OnLevelUnloading()
        {
            OnGameQuit();
        }

        public virtual void OnReleased()
        {
        }

        protected virtual void OnGameStarted()
        {
        }

        protected virtual void OnNewGameStarted()
        {
        }

        protected virtual void OnGameLoaded()
        {
        }

        protected virtual void OnNewAssetStarted()
        {
        }

        protected virtual void OnAssetLoaded()
        {
        }

        protected virtual void OnNewMapStarted()
        {
        }

        protected virtual void OnMapLoaded()
        {
        }

        protected virtual void OnGameQuit()
        {
        }
    }
}
