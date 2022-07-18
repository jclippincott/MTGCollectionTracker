using TCGPlayerApiWrapper.Services;

namespace TCGPlayerApiWrapper {
    public static class Program {
        public static void Main() {
            ManualCardEntryService.StartCardEntryPrompt();
        }

        
    }
}