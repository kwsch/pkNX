#pragma once
#include "../pkNX.HashDecoderConsole/HashDecoder.h"

#include <msclr/marshal_cppstd.h>

using namespace System;

namespace pkNXHashDecoder {
    private ref class FnvHashDecoderWrapper
    {
    public:
        std::unordered_map<uint64_t, std::string> StartDecoding(int length, int startChar, std::string_view prefix)
        {
            return nativeHandle_->StartDecoding(length, startChar, prefix);
        }

        property int AllowedCharCount
        {
            int get()
            {
                return FnvHashDecoder::AllowedCharsCount;
            }
        }

    private:
        FnvHashDecoder* nativeHandle_ = new FnvHashDecoder();
    };

    public ref class FnvHashDecoderLib
    {
    public:
        static System::Collections::Generic::Dictionary<System::UInt64, System::String^>^ StartDecoding(int length, int startChar, System::String^ prefix)
        {
            FnvHashDecoderWrapper decoder{};
            const std::string p = msclr::interop::marshal_as<std::string>(prefix);
            const auto map = decoder.StartDecoding(length, startChar, p);

            auto dict = gcnew System::Collections::Generic::Dictionary<System::UInt64, System::String^>();

            for (const auto& entry : map)
                dict->Add(entry.first, gcnew System::String(entry.second.c_str()));

            return dict;
        }

        static property int AllowedCharCount
        {
            int get()
            {
                return FnvHashDecoder::AllowedCharsCount;
            }
        }
    };
}
