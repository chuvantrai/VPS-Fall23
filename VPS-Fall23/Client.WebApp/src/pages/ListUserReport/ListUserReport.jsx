/* eslint-disable react-hooks/exhaustive-deps */
import { Table, Button, Pagination, Select, Tooltip } from 'antd';
import { QuestionCircleOutlined, FilterOutlined } from '@ant-design/icons';
import { useEffect, useRef, useState } from 'react';

import useReportServices from '@/services/reportServices';

function ListReport() {
  const service = useReportServices();

  const columns = [
    {
      title: 'Kiểu báo cáo',
      dataIndex: 'typeName',
      key: 'typeName',
    },
    {
      title: 'Người tạo',
      dataIndex: 'email',
      key: 'email',
    },
    {
      title: 'Thời gian tạo',
      dataIndex: 'createdAt',
      key: 'createdAt',
    },
    {
      title: 'Nội dung',
      key: 'content',
      dataIndex: 'content',
      width: '20%',
    },
    {
      title: 'Trạng thái',
      dataIndex: 'status',
      key: 'status',
    },
  ];

  const [data, setData] = useState([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [totalItems, setTotalItems] = useState(0);
  const [typeOptions, setTypeOptions] = useState([]);
  const [filterType, setFilterType] = useState('');

  const handleChangePage = (page) => {
    setPageNumber(page);
  };

  const loadData = () => {
    service.getReportForAdmin(pageNumber).then((res) => {
      setData(res.data.data);
      setTotalItems(res.data.totalCount);
    });
  };

  const loadTypeOptions = () => {
    service.loadReportType().then((res) => {
      setTypeOptions(res.data);
    });
  };

  const firstUpdate = useRef(true);
  useEffect(() => {
    if (firstUpdate.current) {
      firstUpdate.current = false;
      loadData();
      return;
    }

    if (filterType === undefined) {
      loadData();
    } else {
      handleFilterReport();
    }
  }, [pageNumber]);

  useEffect(() => {
    loadTypeOptions();
  }, []);

  const handleFilterReport = () => {
    service.filterReport(pageNumber, 10, filterType).then((res) => {
      setData(res.data.data);
      setTotalItems(res.data.totalCount);
    });
  };

  const onChangeType = (value) => {
    setFilterType(value);
  };

  const filterOption = (input, option) => (option?.label ?? '').toLowerCase().includes(input.toLowerCase());

  return (
    <div className="w-[100%]">
      <div className="mb-[16px] flex items-center">
        <p className="mr-[16px]">
          Lọc{' '}
          <Tooltip title="Lọc theo trạng thái báo cáo">
            <QuestionCircleOutlined />
          </Tooltip>{' '}
          :
        </p>
        <Select
          className="w-80"
          showSearch
          placeholder="kiểu báo cáo"
          optionFilterProp="children"
          allowClear
          onChange={onChangeType}
          filterOption={filterOption}
          options={typeOptions}
        />
        <Button className="ml-[10px]" type="primary" icon={<FilterOutlined />} onClick={handleFilterReport}>
          Áp dụng
        </Button>
      </div>

      <Table columns={columns} dataSource={data} pagination={false} />
      <div className="py-[16px] flex flex-row-reverse pr-[24px]">
        <Pagination current={pageNumber} onChange={handleChangePage} total={totalItems} showSizeChanger={false} />
      </div>
    </div>
  );
}

export default ListReport;
