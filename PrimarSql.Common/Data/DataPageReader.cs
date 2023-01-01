using System.Collections;

namespace PrimarSql.Common.Data;

public class DataPageReader : IEnumerable<object[]?>, IDisposable
{
    public object[]? Current { get; private set; }

    public int ColumnCount => _pageProvider.ColumnCount;

    public int Affected => _pageProvider.Affected;

    // TODO: to Column type
    public string[] Columns => _columns ?? _pageProvider.Columns;

    private readonly DataPageProvider _pageProvider;
    private readonly string[]? _columns;
    private bool _isDisposed;
    private bool _isClosed;
    private int _index;
    private IDataPage? _currentPage;

    public DataPageReader(DataPageProvider pageProvider, string[]? columns = null)
    {
        _pageProvider = pageProvider;
        _columns = columns;
    }

    public bool Read()
    {
        if (_isClosed)
            return false;

        if (_currentPage is null || _index >= _currentPage.DataCount)
        {
            if (!_pageProvider.Next())
            {
                _isClosed = true;
                return false;
            }

            _currentPage = _pageProvider.Current;

            if (_currentPage is null || _currentPage.DataCount == 0)
            {
                _isClosed = true;
                return false;
            }
        }

        Current = _currentPage.Get(_index++);
        return true;
    }

    public void Dispose()
    {
        if (_isDisposed)
            return;

        _pageProvider.Dispose();
        _isDisposed = true;

        GC.SuppressFinalize(this);
    }

    public IEnumerator<object[]?> GetEnumerator()
    {
        return new DataPageReaderEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private class DataPageReaderEnumerator : IEnumerator<object[]?>
    {
        private readonly DataPageReader _reader;

        public DataPageReaderEnumerator(DataPageReader reader)
        {
            _reader = reader;
        }

        public bool MoveNext()
        {
            return _reader.Read();
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public object[]? Current => _reader.Current;

        object? IEnumerator.Current => Current;

        public void Dispose()
        {
            _reader.Dispose();
        }
    }
}
