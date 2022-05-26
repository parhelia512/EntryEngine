// ����

// ���ҷ��������ĵ�һ��Ԫ�أ�û�з��ϵķ���undefined
// Boolean func(value)��func�ɲ�������ʱ���ص�һ��Ԫ��
Object.prototype._First = function(func) {
    for (var value of this)
        if (!func || func(value))
            return value;
}
// ���ҷ������������һ��Ԫ�أ�û�з��ϵķ���undefined
// Boolean func(value)��func�ɲ�������ʱ�������һ��Ԫ��
Object.prototype._Last = function(func) {
    if (!func && this instanceof Array) {
        if (this.length == 0)
            return undefined;
        else
            return this[this.length - 1];
    } else {
        var last;
        for (var value of this)
            if (!func || func(value))
                last = value;
        return last;
    }
}
// ���ҷ�������������Ԫ�أ�û�з��ϵķ���undefined
// Boolean func(value)
Object.prototype._Where = function*(func) { 
    for (var value of this) 
        if (func(value))
            yield value;
}
// ��������Ԫ�ؾ�Ϊtrueʱ��Ϊtrue��������Ϊtrue
// Boolean func(value)
Object.prototype._All = function(func) { 
	for (var value of this)
		if (!func(value))
			return false;
	return true;
}
// ��������Ԫ��Ϊtrueʱ��Ϊtrue��������Ϊfalse
// Boolean func(value)
Object.prototype._Any = function(func) { 
	for (var value of this)
		if (func(value))
			return true;
	return false;
}


// ת������������

// ѡ�������Ԫ���е�ĳ���ֶ���Ϊ�µĵ�����
// Any func(value)
Object.prototype._Select = function*(func) { 
    for (var value of this) 
        yield func(value);
}
// ���ϵ�����Ԫ���е�ĳ���������ֶ���Ϊ�µĵ�����
// Any func(value)
Object.prototype._SelectMany = function*(func) {
    for (var value of this) {
        var many = func(value);
        if (many)
            for (var value2 of many)
                yield value2;
    }
}
// ������ͬ����Ԫ�ط��飬�൱���ֵ䣬����Array[key, Array[]]
// Any funcKey(value)
// Any funcValue(value)��funcValue�ɲ�����ֵ����ԭʼ��������Ԫ��
Object.prototype._GroupBy = function(funcKey, funcValue) {
    var group = [];
    var key;
    for (var value of this) {
        key = funcKey(value);
        if (!group[key])
            group[key] = [];
        if (funcValue)
            group[key].push(funcValue(value));
        else
            group[key].push(value);
    }
    return group;
}
// ��������ת�������飬��ԭ�������飬Ҳ�᷵��һ��������
Object.prototype._ToArray = function() {
    var array = [];
    for (var value of this)
        array.push(value);
    return array;
}

// �ۺϺ���

// ��������������Ԫ�ظ���
// Boolean func(value)��func�ɲ�������ʱ���ص�������Ԫ������
Object.prototype._Count = function(func) {
    if (!func && this instanceof Array)
        return this.length;
    var count = 0;
    for (var value of this)
        if (!func || func(value))
            count++;
    return count;
}
// ������������Ԫ�ص�ƽ��ֵ
// Number func(value)��func�ɲ�������ʱthis������Number���͵ĵ�����
Object.prototype._Avg = function(func) {
    var sum = 0;
    var count = 0;
    if (func) {
	    for (var value of this) {
            sum += func(value);
            count++;
        }
    } else {
        for (var value of this) {
            sum += value;
            count++;
        }
    }
    if (count)
        return sum / count;
    else
        return 0;
}
// ������������Ԫ�ص��ۼ�ֵ
// Number func(value)��func�ɲ�������ʱthis������Number���͵ĵ�����
Object.prototype._Sum = function(func) {
    var sum = 0;
    if (func)
	    for (var value of this)
            sum += func(value);
    else
        for (var value of this)
            sum += value;
    return sum;
}
// �������ֵ�����ֵ��Ӧ��Ԫ��
// Number func(value)��func�ɲ�������ʱthis������Number���͵ĵ�����
// e��Ĭ��Ϊtrue��Ϊtrueʱ����Ԫ�أ����򷵻���ֵ
Object.prototype._Max = function(func, e = true) {
    // ���ֵ��Ӧ�Ķ���
    var result;
    if (func) {
        // ���ֵ
        var rvalue;
        var temp;
        for (var value of this) {
            temp = func(value);
            if (rvalue == undefined || temp > rvalue) {
                rvalue = temp;
                result = value;
            }
        }
        if (!e)
            result = rvalue;
    } else {
        for (var value of this) {
            if (result == undefined || value > result)
                result = value;
        }
    }
    return result;
}
// ������Сֵ����Сֵ��Ӧ��Ԫ��
// Number func(value)��func�ɲ�������ʱthis������Number���͵ĵ�����
// e��Ĭ��Ϊtrue��Ϊtrueʱ����Ԫ�أ����򷵻���ֵ
Object.prototype._Min = function(func, e = true) {
    // ��Сֵ��Ӧ�Ķ���
    var result;
    if (func) {
        // ���ֵ
        var rvalue;
        var temp;
        for (var value of this) {
            temp = func(value);
            if (rvalue == undefined || temp < rvalue) {
                rvalue = temp;
                result = value;
            }
        }
        if (!e)
            result = rvalue;
    } else {
        for (var value of this) {
            if (result == undefined || value < result)
                result = value;
        }
    }
    return result;
}


// ����

// �ϲ���һ��������
Object.prototype._Concat = function*(iterator) {
    for (var value of this)
        yield value;
    for (var value of iterator)
        yield value;
}
// ����������ָ��������Ԫ�أ�Ȼ�󷵻�ʣ���Ԫ��
Object.prototype._Skip = function*(count) {
    if (count <= 0)
        return this;
    if (this instanceof Array) {
        for (var i = count; i < this.length; i++)
            yield this[i];
    } else {
        for (var value of this) {
            count--;
            if (count < 0)
                yield value;
        }
    }
}
// �����еĿ�ͷ����ָ������������Ԫ��
Object.prototype._Take = function*(count) {
    for (var value of this) {
        count--;
        if (count >= 0)
            yield value;
        else
            break;
    }
}
// ֻҪ����ָ���������������������е�Ԫ�أ�Ȼ�󷵻�ʣ��Ԫ��
// Boolean func(value)
Object.prototype._SkipWhile = function*(func) {
    var skip = true;
    for (var value of this) {
        if (skip) {
            skip = func(value);
            if (skip)
                continue;
        }
        yield value;
    }
}
// ֻҪ����ָ�����������ͻ᷵�����е�Ԫ��
// Boolean func(value)
Object.prototype._TakeWhile = function*(func) {
    for (var value of this) {
        if (func(value))
            yield value;
        else
            break;
    }
}