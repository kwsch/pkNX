#pragma once
#include "pch.h"
#include <chrono>
#include <iostream>

namespace pkNXHashDecoder {

    class Timer {
    public:
        /// <summary>Construct a new timer</summary>
        Timer() {
            Reset();
        }

        /// <summary>Resets the timer to now</summary>
        void Reset() {
            start_ = Clock::now();
        }

        auto Elapsed() const {
            return FMicro(Clock::now() - start_).count();
        }

        auto ElapsedMillis() const {
            return FMili(Clock::now() - start_).count();
        }

    private:
        using Clock = std::chrono::high_resolution_clock;
        using FMicro = std::chrono::duration<float, std::micro>;
        using FMili = std::chrono::duration<float, std::milli>;

        Clock::time_point start_;
    };

    class LoopTimer : Timer {
    public:
        LoopTimer() = default;

        /// <summary>Resets the timer to now</summary>
        void Reset() {
            loops = 0;
        }

        auto ElapsedAverage() const {
            return Elapsed() / static_cast<float>(loops);
        }

        auto ElapsedAverageMillis() const {
            return totalTime / static_cast<float>(loops);
        }

        void Loop() {
            totalTime += ElapsedMillis();
            ++loops;
            Timer::Reset();
        }

        void Print()
        {
            std::cout << "[Timer]: " << loops << " loops, " << ElapsedAverageMillis() << "ms avg.\n";
        }

    private:
        int loops = 0;
        float totalTime = 0.0f;
    };

    class ScopedTimer {
    public:
        ScopedTimer(const std::string_view& name)
            : _name(name) {
        }

        ~ScopedTimer() {
            std::cout << "[Timer]: " << _name << " - " << _timer.ElapsedMillis() << "ms";
        }

    private:
        std::string _name;
        Timer _timer;
    };
} // namespace pix::foundation
