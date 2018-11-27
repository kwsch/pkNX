for %%f in (*.json) do call flatc -b encount_data.fbs %%f --defaults-json
pause