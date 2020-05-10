namespace TorrentCliTest
{
    using System.IO;
    using System.Linq;
    using NUnit.Framework;
    using TorrentClient;
    using TorrentClient.Extensions;
    using TorrentClient.PeerWireProtocol;

    public class PersistenceManagerTest
    {
        [Test, Explicit]
        [TestCase(@"..\..\..\TorrentClientTest.Data\Torrent10.torrent", @".\Test")]
        public void PersistenceManager_Test(string sourcePath, string destPath)
        {
            if (TorrentInfo.TryLoad(sourcePath, out TorrentInfo torrent))
            {
                using (PersistenceManager src = new PersistenceManager(Path.GetDirectoryName(sourcePath), torrent.Length, torrent.PieceLength, torrent.PieceHashes, torrent.Files))
                {
                    using PersistenceManager dest = new PersistenceManager(destPath, torrent.Length, torrent.PieceLength, torrent.PieceHashes, torrent.Files);
                    for (int pieceIndex = 0; pieceIndex < torrent.PiecesCount; pieceIndex++)
                    {
                        dest.Put(torrent.Files, torrent.PieceLength, pieceIndex, src.Get(pieceIndex));
                    }

                    Assert.IsTrue(dest.Verify().All(x => x == PieceStatus.Present));
                }

                destPath.DeleteDirectoryRecursively();
            }
            else
            {
                Assert.Fail();
            }
        }
    }
}
