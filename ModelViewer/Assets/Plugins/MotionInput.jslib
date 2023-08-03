mergeInto(LibraryManager.library, {
  Unity_MediaPipeImport: function() {
    // Import the modules here.
    import {
      PoseLandmarker,
      FilesetResolver,
      DrawingUtils
    } from "https://cdn.skypack.dev/@mediapipe/tasks-vision@0.10.0";

    window.alert("Hello, world!");
  }
});