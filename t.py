a = str(input())
kirills = list(map(int, input().split()))
c = 2
l = len(kirills)
while l:
    if kirills[c] == kirills[c + 1] == kirills[c + 2]:
        kirills.pop(c)
        l -= 1
    l -= 1
    c += 1
print(kirills)
