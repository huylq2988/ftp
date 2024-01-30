select p.Folder, l.LogDate, l.LogTime, l.TagName, l.LastValue from BwAnalogTable l
					INNER JOIN Params p ON l.TagName = p.TagName
					where p.Interval is not null and p.Enable=1 --and Folder = 'S1' 
					and l.LogDate = CONVERT(VARCHAR, DATEADD(MI, -1, GETDATE()), 11) 
					and l.LogTime = SUBSTRING(CONVERT(VARCHAR, DATEADD(MI, -1, GETDATE()), 20), 12, 5) + ':00';