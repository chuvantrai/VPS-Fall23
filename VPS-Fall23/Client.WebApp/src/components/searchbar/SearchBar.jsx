import PropTypes from 'prop-types';
import { Cascader } from 'antd';

const options = [
  {
    value: 'zhejiang',
    label: 'Zhejiang',
  },
  {
    value: 'jiangsu',
    label: 'Jiangsu',
  },
];
const onChange = (value, selectedOptions) => {
  console.log(value, selectedOptions);
};
const filter = (inputValue, path) =>
  path.some((option) => option.label.toLowerCase().indexOf(inputValue.toLowerCase()) > -1);

function SearchBar({ data }) {
  return (
    <Cascader
      options={data}
      onChange={onChange}
      placeholder="Please select"
      showSearch={{
        filter,
      }}
      onSearch={(value) => console.log(value)}
    ></Cascader>
  );
}

SearchBar.propTypes = {
  data: PropTypes.arrayOf(PropTypes.shape({ label: PropTypes.string, value: PropTypes.string }))
}

export default SearchBar;
