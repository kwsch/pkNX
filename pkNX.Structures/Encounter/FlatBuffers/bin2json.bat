for %%f in (*.bin) do call flatc -t encount_data.fbs -- %%f --defaults-json --raw-binary
pause