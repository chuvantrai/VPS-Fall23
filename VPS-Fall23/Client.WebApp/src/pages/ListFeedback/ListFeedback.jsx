/* eslint-disable react-hooks/exhaustive-deps */
import { Table, Button, Pagination, Rate, Select, Tooltip } from 'antd';
import { QuestionCircleOutlined, FilterOutlined } from '@ant-design/icons';
import { useEffect, useRef, useState } from 'react';

import useFeedbackServices from '@/services/feedbackServices';
import useParkingZoneService from '@/services/parkingZoneService';
import { getAccountJwtModel } from '@/helpers';
import ReplyModal from './components/ReplyModal';

function ListFeedback() {
  const service = useFeedbackServices();
  const parkingZoneService = useParkingZoneService();
  const account = getAccountJwtModel();

  const columns = [
    {
      title: 'Bãi đỗ xe',
      dataIndex: 'parkingZoneName',
      key: 'parkingZoneName',
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
      title: 'Đánh giá',
      key: 'rate',
      dataIndex: 'rate',
      render: (_, { rate }) => <Rate disabled value={rate} />,
    },
    {
      title: 'Nội dung đánh giá',
      key: 'content',
      dataIndex: 'content',
      width: '20%',
    },
    {
      title: 'Phản hồi',
      key: 'replies',
      dataIndex: 'replies',
      width: '20%',
      render: (_, { replies }) => replies.map((reply) => reply.content),
    },
    {
      title: 'Action',
      key: 'action',
      render: (_, record) => (
        <>
          {record.replies.length == 0 && (
            <Button
              type="primary"
              onClick={() => {
                handleOpenReplyModal(record.id);
              }}
            >
              Trả lời bình luận
            </Button>
          )}
          {record.replies.length > 0 && (
            <Button type="primary" disabled>
              Trả lời bình luận
            </Button>
          )}
        </>
      ),
    },
  ];

  const [data, setData] = useState([]);
  const [replyModalOpen, setReplyModalOpen] = useState(false);
  const [feedbackId, setFeedbackId] = useState('');
  const [pageNumber, setPageNumber] = useState(1);
  const [totalItems, setTotalItems] = useState(0);
  const [parkingZoneOptions, setParkingZoneOptions] = useState([]);
  const [filterRating, setFilterRating] = useState('');
  const [filterParkingZone, setFilterParkingZone] = useState('');

  const handleOpenReplyModal = (feedbackId) => {
    setFeedbackId(feedbackId);
    setReplyModalOpen(true);
  };

  const handleChangePage = (page) => {
    setPageNumber(page);
  };

  const loadData = () => {
    service.getFeedbackForOwner(account.UserId, pageNumber).then((res) => {
      setData(res.data.data);
      setTotalItems(res.data.totalCount);
    });
  };

  const loadParkingZoneOptions = () => {
    parkingZoneService.getApprovedParkingZoneByOwnerId(account.UserId).then((res) => {
      setParkingZoneOptions(res.data);
    });
  };

  const firstUpdate = useRef(true);
  useEffect(() => {
    if (firstUpdate.current) {
      firstUpdate.current = false;
      loadData();
      return;
    }

    if (filterParkingZone === undefined && filterRating === undefined) {
      loadData();
    } else {
      handleFilterFeedback();
    }
  }, [pageNumber]);

  useEffect(() => {
    loadParkingZoneOptions();
  }, []);

  const handleReplyFeedback = (values) => {
    service.addReplyToFeedback(values);
    setReplyModalOpen(false);
    loadData();
  };
  const handleFilterFeedback = () => {
    service.filterFeedback(account.UserId, pageNumber, 10, filterParkingZone, filterRating).then((res) => {
      setData(res.data.data);
      setTotalItems(res.data.totalCount);
    });
  };

  const onChangeParkingZone = (value) => {
    setFilterParkingZone(value);
  };
  const onChangeRating = (val) => {
    setFilterRating(val);
  };

  // Filter `option.label` match the user type `input`
  const filterOption = (input, option) => (option?.label ?? '').toLowerCase().includes(input.toLowerCase());

  return (
    <div className="w-[100%]">
      <div className="mb-[16px] flex items-center">
        <p className="mr-[16px]">
          Lọc{' '}
          <Tooltip title="Lọc theo bãi đỗ xe hoặc rating">
            <QuestionCircleOutlined />
          </Tooltip>{' '}
          :
        </p>
        <Select
          className="w-80"
          showSearch
          placeholder="Bãi đỗ xe"
          optionFilterProp="children"
          allowClear
          onChange={onChangeParkingZone}
          filterOption={filterOption}
          options={parkingZoneOptions}
        />
        <Select
          className="w-80 ml-[10px]"
          placeholder="Rating"
          allowClear
          onChange={onChangeRating}
          options={[
            {
              value: '1',
              label: <Rate disabled defaultValue={1} />,
            },
            {
              value: '2',
              label: <Rate disabled defaultValue={2} />,
            },
            {
              value: '3',
              label: <Rate disabled defaultValue={3} />,
            },
            {
              value: '4',
              label: <Rate disabled defaultValue={4} />,
            },
            {
              value: '5',
              label: <Rate disabled defaultValue={5} />,
            },
          ]}
        />
        <Button className="ml-[10px]" type="primary" icon={<FilterOutlined />} onClick={handleFilterFeedback}>
          Áp dụng
        </Button>
      </div>

      <Table columns={columns} dataSource={data} pagination={false} />
      <div className="py-[16px] flex flex-row-reverse pr-[24px]">
        <Pagination current={pageNumber} onChange={handleChangePage} total={totalItems} showSizeChanger={false} />
      </div>

      <ReplyModal
        open={replyModalOpen}
        feedbackId={feedbackId}
        onCreate={handleReplyFeedback}
        onCancel={() => {
          setReplyModalOpen(false);
        }}
      />
    </div>
  );
}

export default ListFeedback;
