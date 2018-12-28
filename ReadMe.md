# TuckBytesInCode #

Experimenting with ways to stuff binary data into source code

## math ##
There seems be some unerlying math for any base converison formula.

The next table shows that bytes in and bytes out can be used to calculate the base using the formula: `ceiling(2 ^ (8 * bytes in / chars out))`
Ceiling is used since we need a whole number of characters in a base

| base name  | bytes-in | chars-out | calculated base |
|------------|----------|-----------|-----------------|
| base 64    | 3        | 4         | 64              |
| base 85    | 4        | 5         | 85              |
| base 91    | 13       | 16        | 91              |
| base 14938 | 26       | 15        | 14938           |

This next table shows adjusting the chars-out to fit a multiple of 8. i'm choosing 8 so that things fit nicely within bytes

| base  | co-LCM  | mult | bi * mult | 2 ^ bi * mult | log2(base) |
|-------|---------|------|-----------|---------------|------------|
| 64    | 8       | 2    | 6         | 64            | 6          |
| 85    | 40      | 8    | 32        | 4294967296    | 6.40939093 |
| 91    | 16      | 8    | 13        | 8192          | 6.50779464 |
| 14938 | 120     | 1    | 208       | 4.113761E+062 | 13.8666993 |

* co-LCM:        least common multiple between chars-out and 8
* mult:          co-LCM / co - what we would need to multiply bytes-in to preserve the ratio
* bi * mult:     bytes-in * multiplier
* 2 ^ bi * mult: number of characters needed to represent the adjusted bytes-in
* log2(base):    number of bits needed to fit the number base

What's notable about this table is that base 64 fits exacly - so we could read 6 bits and directly map to the output character map - no base conversion needed.

### ratios ###
What if we wanted to determine the bytes-in to chars-out ratio for any base ?
We can use the already esablished formula `base = 2 ^ (8 * bytes in / chars out)`
replacing the bytes in / chars out with just `r` we get `base = 2 ^ (8 * r)`
solving for `r` we get `r = log(base) / (8 * log(2))`

Here's the ratios for the bases being used so far compared to the bytes-in / chars-out ratio

| base  | log(base)/8*log2  | ratio   | decimal          | base no ceiling  |
|-------|-------------------|---------|------------------|------------------|
| 64    | 0.75              | 3 / 4   | 0.75             | 64               |
| 85    | 0.801173867017213 | 4 / 5   | 0.8              | 84.4485062894653 |
| 91    | 0.813474330024837 | 13 / 16 | 0.8125           | 90.5096679918781 |
| 14938 | 1.733337422860250 | 26 / 15 | 1.73333333333333 | 14937.6612525378 |

You can see that the fractions that were picked produce decimals that are just slightly below the target ratio. Also notice that when we don't ceiling the ratio to base calculation, that the base number is close to the whole number base.

The difficulty with finding a bytes-in / chars-out fraction starting from just the ratio is that it's somewhat difficult to find a fraction that fits - we want a ratio that as close as possible without going over but that still uses small-ish numbers.

There are several algorithms for finding fractions that fit our decimal. I've included some links with more information. One method I've been using with excel is a brute force search which is also described below.

### links related to converting decimal to fraction ###
* [Algorithm for simplifying decimal to fractions](https://stackoverflow.com/questions/5124743/algorithm-for-simplifying-decimal-to-fractions)
* [Converting decimal to fraction c++](https://stackoverflow.com/questions/26643695/converting-decimal-to-fraction-c)
* [Decimal to fraction conversion](https://stackoverflow.com/questions/9386422/decimal-to-fraction-conversion)

### brute force fraction using a spread sheet ###
These are the formulas use in the spread sheet. formulas #3-6 are meant to be copied to each row in the spread sheet. Most spread sheets support coping entries by highlighting the cells on row 4 and 'draggin down' using the small box in the bottom-right corner.

| #  | name            | formula                              |
|----|-----------------|--------------------------------------|
| #1 | ratio from base | =LOG(A2)/(8*LOG(2))                  |
| #2 | min matcher     | =MATCH(MIN(D4:D33),D4:D33,0)+3       |
| #3 | calc numerator  | =FLOOR(B4*$B$2)                      |
| #4 | ratio           | =A4/B4                               |
| #5 | difference      | =ROUND(1000*ABS(C4-$B$2),2)          |
| #6 | closest base    | =CEILING(POWER(2,8*A4/B4))           |

here's an example of the spread sheet using base 91 (in A2). The best row turns out to be 19 with the fraction 13 / 16 which matches the optimum values from before.

|      | _A_            | _B_           | _C_           | _D_           | _E_             |
|------|----------------|---------------|---------------|---------------|-----------------|
|  _1_ | base           | base ratio #1 |               | minimum #2    |                 |
|  _2_ | 91             | 0.81347433002 |               | 19            |                 |
|  _3_ | numerator #3   | denominator   | ratio #4      | difference #5 | closest base #6 |
|  _4_ | 0              | 1             | 0             | 813.47        | 1               |
|  _5_ | 1              | 2             | 0.5           | 313.47        | 16              |
|  _6_ | 2              | 3             | 0.66666666666 | 146.81        | 41              |
|  _7_ | 3              | 4             | 0.75          | 63.47         | 64              |
|  _8_ | 4              | 5             | 0.8           | 13.47         | 85              |
|  _9_ | 4              | 6             | 0.66666666666 | 146.81        | 41              |
| _10_ | 5              | 7             | 0.71428571428 | 99.19         | 53              |
| _11_ | 6              | 8             | 0.75          | 63.47         | 64              |
| _12_ | 7              | 9             | 0.77777777777 | 35.7          | 75              |
| _13_ | 8              | 10            | 0.8           | 13.47         | 85              |
| _14_ | 8              | 11            | 0.72727272727 | 86.2          | 57              |
| _15_ | 9              | 12            | 0.75          | 63.47         | 64              |
| _16_ | 10             | 13            | 0.76923076923 | 44.24         | 72              |
| _17_ | 11             | 14            | 0.78571428571 | 27.76         | 79              |
| _18_ | 12             | 15            | 0.8           | 13.47         | 85              |
| _19_ | 13             | 16            | 0.8125        | 0.97          | 91              |
| _20_ | 13             | 17            | 0.76470588235 | 48.77         | 70              |
| _21_ | 14             | 18            | 0.77777777777 | 35.7          | 75              |
| _22_ | 15             | 19            | 0.78947368421 | 24            | 80              |
| _23_ | 16             | 20            | 0.8           | 13.47         | 85              |

## notes ##
