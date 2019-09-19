//-----------------------------------------------------------------------------
// Copyright 2012 Masanori Morise
// Author: mmorise [at] yamanashi.ac.jp (Masanori Morise)
// Last update: 2017/02/01
//-----------------------------------------------------------------------------
#ifndef WORLD_AUDIOIO_H_
#define WORLD_AUDIOIO_H_

#include "world.h"

WORLD_BEGIN_C_DECLS

//-----------------------------------------------------------------------------
// wavwrite() write a .wav file.
// Input:
//   x          : Input signal
//   x_ength : Signal length of x [sample]
//   fs         : Sampling frequency [Hz]
//   nbit       : Quantization bit [bit]
//   filename   : Name of the output signal.
// Caution:
//   The variable nbit is not used in this function.
//   This function only supports the 16 bit.
//-----------------------------------------------------------------------------
WORLD_API void wavwrite(const double *x, int x_length, int fs, int nbit,
  const char *filename);

//-----------------------------------------------------------------------------
// GetAudioLength() returns the length of .wav file.
// Input:
//   filename     : Filename of a .wav file.
// Output:
//   The number of samples of the file .wav
//-----------------------------------------------------------------------------
WORLD_API int GetAudioLength(const char *filename);

//-----------------------------------------------------------------------------
// wavread() read a .wav file.
// The memory of output x must be allocated in advance.
// Input:
//   filename     : Filename of the input file.
// Output:
//   fs           : Sampling frequency [Hz]
//   nbit         : Quantization bit [bit]
//   x            : The output waveform.
//-----------------------------------------------------------------------------
WORLD_API void wavread(const char* filename, int *fs, int *nbit, double *x);

WORLD_END_C_DECLS

#endif  // WORLD_AUDIOIO_H_
