using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using DNT.Engine.Core;
using DNT.Engine.Core.Messaging;
using DNT.Engine.Core.Validation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DNT.AsteroidChallenge.App
{
    public struct AsteroidDestroyedScore
    {
        public AsteroidDestroyedScore(Int32 points)
        {
            _points = points;
        }

        public Int32 Points
        {
            get { return _points; }
        }
        private readonly Int32 _points;
    }

    public class Scorer : DrawableComponent<SpriteFont>
    {
        public Scorer(Scene scene, String assetName)
            : base(scene)
        {
            Verify.That(assetName).Named("assetName").IsNotNullOrWhiteSpace();
            _assetName = assetName;

            Messenger.Register<AsteroidDestroyedScore>(this, OnAsteroidDestroyed);
            Messenger.Register<ShipDestroyed>(this, OnShipDestroyed);
        }

        private void OnAsteroidDestroyed(Message<AsteroidDestroyedScore> message)
        {
            // TODO: collect only if ship is not destroyed.
            _currentScore += message.Content.Points;
        }

        private void OnShipDestroyed(Message<ShipDestroyed> message)
        {
            var history = LoadHistory();
            history.Add(new ScoreHistoryElement
            {
                Score = _currentScore,
                GamerTag = Scene.CurrentGamer.IsNull() || Scene.CurrentGamer.Gamertag.IsNull()
                ? "Guest"
                : Scene.CurrentGamer.Gamertag
            });

            var newHistory = history.OrderByDescending(i => i.Score).Take(HistorySize);
            SaveHistory(newHistory);
        }

        private static IList<ScoreHistoryElement> LoadHistory()
        {
            IList<ScoreHistoryElement> history;

            using (var file = IsolatedStorageFile.GetUserStoreForApplication())
            using (var stream = file.OpenFile(ScoreHistoryFileName, FileMode.OpenOrCreate))
            using (var reader = new BinaryReader(stream, Encoding.UTF8))
                history = reader.ReadList<ScoreHistoryElement>();

            return history;
        }

        private static void SaveHistory(IEnumerable<ScoreHistoryElement> history)
        {
            using (var file = IsolatedStorageFile.GetUserStoreForApplication())
            using (var stream = file.OpenFile(ScoreHistoryFileName, FileMode.OpenOrCreate))
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(history);
                writer.Flush();
            }
        }

        private const String ScoreHistoryFileName = "AC_SCORE_HISTORY.bin";
        private const Int32 HistorySize = 10;

        private readonly String _assetName;
        private SpriteFont _font;

        public Int32 CurrentScore
        {
            get { return _currentScore; }
        }
        private Int32 _currentScore;

        public override void Load()
        {
            base.Load();

            _font = Scene.Content.Load<SpriteFont>(_assetName);
        }

        protected override void Render(Matrix view, Matrix projection)
        {
            var text = String.Format("Score: {0}", _currentScore.ToString(CultureInfo.InvariantCulture).PadLeft(4, '0'));
            var measure = _font.MeasureString(text);
            var yellowPosition = new Vector2(Scene.Viewport.Width - measure.X, 1.0f) - new Vector2(5.0f);
            var redPosition = yellowPosition - new Vector2(1.0f);
            Scene.SpriteBatch.Begin();
            Scene.SpriteBatch.DrawString(_font, text, redPosition, Color.Red);
            Scene.SpriteBatch.DrawString(_font, text, yellowPosition, Color.Yellow);
            Scene.SpriteBatch.End();
        }

        protected override SpriteFont Content
        {
            get { return _font; }
        }
    }
}