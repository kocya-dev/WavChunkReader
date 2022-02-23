using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WavChunkReader
{
    internal class RiffReader
    {
        private string filePath;
        private StringBuilder _sb = new StringBuilder();

        public RiffReader(string filePath)
        {
            using FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using BinaryReader br = new BinaryReader(fs);
            this.filePath = filePath;

            _sb.AppendLine($"{br.BaseStream.Position:X8} {"id",20} = {new string(br.ReadChars(4))}");
            _sb.AppendLine($"{br.BaseStream.Position:X8} {"size",20} = {br.ReadUInt32()}");
            _sb.AppendLine($"{br.BaseStream.Position:X8} {"fmt",20} = {new string(br.ReadChars(4))}");

            while (br.BaseStream.Position < br.BaseStream.Length)
            {
                _sb.Append(parseChunk(br));
            }
        }
        private string parseChunk(BinaryReader br)
        {
            StringBuilder sb = new StringBuilder();

            long pos = br.BaseStream.Position;
            string chars = new string(br.ReadChars(4));
            sb.AppendLine($"{pos:X8} {"chunk type",20} = {chars}");

            long chunkPos = br.BaseStream.Position;
            UInt32 chunkDataSize = br.ReadUInt32();
            sb.AppendLine($"{chunkPos:X8} {"size",20} = {chunkDataSize}");

            string chunkData = chars.ToUpper() switch
            {
                "FMT " => parseChunkFmt(br.BaseStream.Position, br, chunkDataSize),
                "FACT" => skipChunk(br.BaseStream.Position, br, chunkDataSize),
                "CUE " => skipChunk(br.BaseStream.Position, br, chunkDataSize),
                "PLST" => skipChunk(br.BaseStream.Position, br, chunkDataSize),
                "LIST" => skipChunk(br.BaseStream.Position, br, chunkDataSize),
                "LABL" => skipChunk(br.BaseStream.Position, br, chunkDataSize),
                "LTXT" => skipChunk(br.BaseStream.Position, br, chunkDataSize),
                "SMPL" => skipChunk(br.BaseStream.Position, br, chunkDataSize),
                "INST" => skipChunk(br.BaseStream.Position, br, chunkDataSize),
                "DATA" => skipChunk(br.BaseStream.Position, br, chunkDataSize),
                "ID3 " => skipChunk(br.BaseStream.Position, br, chunkDataSize),
                _ => string.Empty,
            };
            if (!string.IsNullOrEmpty(chunkData))
            {
                sb.Append(chunkData);
            }
            return sb.ToString();
        }

        private string skipChunk(long pos, BinaryReader br, uint chunkDataSize)
        {
            br.BaseStream.Position += chunkDataSize;
            return String.Empty;
        }
        private string parseChunkFmt(long pos, BinaryReader br, UInt32 chunkDataSize)
        {
            StringBuilder sb = new StringBuilder();
            long posAudioFormat = br.BaseStream.Position;
            UInt16 audioFormat = br.ReadUInt16();
            sb.AppendLine($"{posAudioFormat:X8} {"audio format",20} = {audioFormat}");
            sb.AppendLine($"{br.BaseStream.Position:X8} {"ch",20} = {br.ReadInt16()}");
            sb.AppendLine($"{br.BaseStream.Position:X8} {"freq",20} = {br.ReadUInt32()}");
            sb.AppendLine($"{br.BaseStream.Position:X8} {"byte per sec",20} = {br.ReadUInt32()}");
            sb.AppendLine($"{br.BaseStream.Position:X8} {"block byte size",20} = {br.ReadUInt16()}");
            sb.AppendLine($"{br.BaseStream.Position:X8} {"bit per sample",20} = {br.ReadUInt16()}");
            if (audioFormat != 1)
            {
                sb.AppendLine($"{br.BaseStream.Position:X8} {"ext size",20} = {br.ReadUInt16()}");
                // sb.AppendLine($"{pos} ext = {br.ReadUInt32()}");
            }
            long remainDataSize = (pos + chunkDataSize) - br.BaseStream.Position;
            if (remainDataSize > 0)
            {
                br.BaseStream.Position += remainDataSize;
            }
            return sb.ToString();
        }

        public override string ToString()
        {
            return _sb.ToString();
        }
    }
}
