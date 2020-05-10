namespace TorrentCliTest
{
    using NUnit.Framework;
    using System.Threading;
    using TorrentClient;

    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test ,Explicit]
        public void TestTransferManager()
        {
            TorrentInfo torrent;
            PersistenceManager pm;
            ThrottlingManager tm;
            TransferManager transfer;

            TorrentInfo.TryLoad(@"debian-9.9.0-amd64-netinst.torrent", out torrent);

            tm = new ThrottlingManager();
            tm.WriteSpeedLimit = 1024 * 1024;
            tm.ReadSpeedLimit = 1024 * 1024;

            pm = new PersistenceManager(@"Test\", torrent.Length, torrent.PieceLength, torrent.PieceHashes, torrent.Files);

            transfer = new TransferManager(4000, torrent, tm, pm);
            transfer.Start();

            Thread.Sleep(1000000);
        }
    }
}

